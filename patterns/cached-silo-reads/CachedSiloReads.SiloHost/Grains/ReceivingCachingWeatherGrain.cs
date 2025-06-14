using CachedSiloReads.SiloHost.GrainModel;
using CachedSiloReads.SiloHost.Model;
using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace CachedSiloReads.SiloHost.Grains;

[StatelessWorker(1)]
public class ReceivingCachingWeatherGrain(ILogger<ICachingWeatherGrain> logger) : Grain, IReceivingCachingWeatherGrain
{
    private WeatherCacheItem _theCacheItem = null!;

    ConfiguredCancelableAsyncEnumerable<WeatherForecast> _enumeratorProxy;

    private Task _monitorTask = null!;
    private CancellationTokenSource _stopMonitoringToken = null!;

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _stopMonitoringToken = new();
        using var registration = cancellationToken.Register(_stopMonitoringToken.Cancel);

        _enumeratorProxy = GrainFactory
            .GetGrain<IPushingWeatherGrain>("the-weather-grain")
            .GetForecastUpdates()
            .WithCancellation(_stopMonitoringToken.Token);

        var enumerator = _enumeratorProxy.GetAsyncEnumerator();

        // Wait in OnActivate for the first forecast to ensure the cache can provide on the first incoming call.
        try
        {
            if (await enumerator.MoveNextAsync())
            {
                logger.LogInformation("Cache '{GrainKey}' received initial forecast: {Forecast}", this.GetPrimaryKeyString(), enumerator.Current);
                _theCacheItem = CreateCacheItem(enumerator.Current);
            }
        }
        catch (Exception)
        {
            await enumerator.DisposeAsync();
            _stopMonitoringToken.Dispose();
            throw;
        }

        // Pass the same enumerator on to the monitoring task to have an uninterrupted feed of
        // forecasts using the same enumerating grain call. This should ensure no forecasts are 
        // missed and/or received multiple times between init and monitoring.
        _monitorTask = Task.Factory
            .StartNew(
                () => MonitorUpdates(enumerator),
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Current)
            .Unwrap();

        await base.OnActivateAsync(cancellationToken);
    }

    public override async Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
    {
        try
        {
            _stopMonitoringToken.Cancel();
            await _monitorTask;
        }
        finally
        {
            _stopMonitoringToken.Dispose();
        }

        await base.OnDeactivateAsync(reason, cancellationToken);
    }

    private async Task MonitorUpdates(ConfiguredCancelableAsyncEnumerable<WeatherForecast>.Enumerator enumerator)
    {
        // Demo: wait a while to show no forecasts will be missed. They are stored
        // per subscriber on the pushing grain, waiting to be picked up by the async iterator.
        await Task.Delay(TimeSpan.FromSeconds(5));

        try
        {
            while (await enumerator.MoveNextAsync())
            {
                logger.LogInformation("Cache '{GrainKey}' received updated forecast: {Forecast}", this.GetPrimaryKeyString(), enumerator.Current);
                _theCacheItem = CreateCacheItem(enumerator.Current);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException && ex is not TaskCanceledException)
        {
            logger.LogError(ex, "Error receiving weather updates");
            DeactivateOnIdle();
        }
        finally
        {
            await enumerator.DisposeAsync();
        }
    }

    private static WeatherCacheItem CreateCacheItem(WeatherForecast forecast) => new()
    {
        Forecast = forecast,
        LastUpdated = DateTime.UtcNow
    };

    public ValueTask<WeatherForecast> GetForecast() => ValueTask.FromResult(_theCacheItem.Forecast);

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

