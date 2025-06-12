using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CachedSiloReads.SiloHost.Grains;

[StatelessWorker(1)]
public class ReceivingCachingWeatherGrain : Grain, IReceivingCachingWeatherGrain
{
    private readonly ILogger<ICachingWeatherGrain> _logger;

    /// <summary>
    /// Value guaranteed in OnActivateAsync.
    /// </summary>
    [NotNull]
    private WeatherCacheItem _theCacheItem;

    private Task _monitorTask;
    private readonly CancellationTokenSource _stopMonitoringToken = new();

    public ReceivingCachingWeatherGrain(ILogger<ICachingWeatherGrain> logger)
    {
        _logger = logger;
    }

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        // Ensure cache item received before grain activating completes.
        await foreach (var cacheItem in GetUpdates(cancellationToken))
        {
            _theCacheItem = cacheItem;
            _logger.LogInformation("Cache '{GrainKey}' received initial forecast: {Forecast}", this.GetPrimaryKeyString(), cacheItem.Forecast);
            break;
        }

        _monitorTask = Task.Factory
            .StartNew(
                () => MonitorUpdates(_stopMonitoringToken.Token),
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Current)
            .Unwrap();

        await base.OnActivateAsync(cancellationToken);
    }

    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _stopMonitoringToken.Cancel();

        await _monitorTask;

        await base.OnDeactivateAsync(reason, cancellationToken);
    }

    private async Task MonitorUpdates(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            await foreach (var cacheItem in GetUpdates(cancellationToken))
            {
                _theCacheItem = cacheItem;

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Error receiving weather updates");
            DeactivateOnIdle();
        }
    }

    private async IAsyncEnumerable<WeatherCacheItem> GetUpdates([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var weatherGrain = GrainFactory.GetGrain<IPushingWeatherGrain>("just-one");

        // TODO: add cancellation. Actually this should be solvable soon when https://github.com/dotnet/orleans/issues/8958 is released.
        await foreach (var forecast in weatherGrain.GetForecastUpdates())
        {
            _logger.LogInformation("Cache '{GrainKey}' received updated forecast: {Forecast}", this.GetPrimaryKeyString(), forecast);

            yield return new WeatherCacheItem
            {
                Forecast = forecast,
                LastUpdated = DateTime.UtcNow
            };

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
        }
    }

    public Task<WeatherForecast> GetForecast()
    {
        return Task.FromResult(_theCacheItem.Forecast);
    }

    public Task Deactivate()
    {
        DeactivateOnIdle();
        return Task.CompletedTask;
    }

    private class WeatherCacheItem
    {
        public required WeatherForecast Forecast { get; init; }

        public required DateTime LastUpdated { get; init; }
    }
}
