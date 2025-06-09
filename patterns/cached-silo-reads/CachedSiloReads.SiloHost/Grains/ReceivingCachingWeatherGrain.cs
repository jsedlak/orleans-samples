using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using Orleans.Concurrency;

namespace CachedSiloReads.SiloHost.Grains;

[StatelessWorker(1)]
public class ReceivingCachingWeatherGrain : Grain, IReceivingCachingWeatherGrain
{
    private readonly ILogger<ICachingWeatherGrain> _logger;
    private WeatherCacheItem? _cacheItem;
    private Task _receivingTask;
    private readonly CancellationTokenSource _stopReceivingToken = new();

    public ReceivingCachingWeatherGrain(ILogger<ICachingWeatherGrain> logger)
    {
        _logger = logger;
    }

    public override Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _receivingTask = Task.Factory
            .StartNew(
                () => ReceiveUpdates(_stopReceivingToken.Token),
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Current)
            .Unwrap();

        return base.OnActivateAsync(cancellationToken);
    }

    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        _stopReceivingToken.Cancel();

        await _receivingTask;

        await base.OnDeactivateAsync(reason, cancellationToken);
    }

    private async Task ReceiveUpdates(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        var theWeatherGrain = GrainFactory.GetGrain<IPushingWeatherGrain>("just-one");

        try
        {
            // TODO: add cancellation. Actually this should be solvable soon when https://github.com/dotnet/orleans/issues/8958 is released.
            await foreach (var forecast in theWeatherGrain.GetForecastUpdates())
            {
                _logger.LogInformation("Cache '{GrainKey}' received updated forecast: {Forecast}", this.GetPrimaryKeyString(), forecast);

                _cacheItem = new WeatherCacheItem
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
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Error receiving weather updates");
            DeactivateOnIdle();
        }
    }

    public Task<WeatherForecast?> GetForecast()
    {
        return Task.FromResult(_cacheItem?.Forecast);
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
