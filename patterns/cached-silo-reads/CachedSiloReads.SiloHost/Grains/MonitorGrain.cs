namespace CachedSiloReads.SiloHost.Grains;

using CachedSiloReads.SiloHost.GrainModel;
using Orleans.Concurrency;
using System.Runtime.CompilerServices;

[StatelessWorker(1, removeIdleWorkers: false)]
public abstract class MonitorGrain<T>(ILogger logger) : Grain, IMonitorGrain<T> where T : notnull
{
    private CacheItem _current = null!;
    ConfiguredCancelableAsyncEnumerable<T> _enumeratorProxy;

    private Task _monitorTask = null!;
    private CancellationTokenSource _stopMonitoringToken = null!;

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        _stopMonitoringToken = new();
        using var tokenRegistration = cancellationToken.Register(_stopMonitoringToken.Cancel);

        _enumeratorProxy = GetUpdates().WithCancellation(_stopMonitoringToken.Token);
        var enumerator = _enumeratorProxy.GetAsyncEnumerator();

        // Wait in OnActivate for the first value to ensure the cache can provide on the first incoming call.
        try
        {
            if (await enumerator.MoveNextAsync())
            {
                logger.LogInformation("{GrainId} received initial value: {Value}", this.GetGrainId(), enumerator.Current);
                _current = CreateCacheItem(enumerator.Current);
            }
        }
        catch (Exception)
        {
            // Need early cleanup because OnDeactivate will not be called.
            await enumerator.DisposeAsync();
            _stopMonitoringToken.Dispose();
            throw;
        }

        // Pass the enumerator instance on to the monitoring task to have an uninterrupted feed 
        // of updates using the same enumerating grain call. This ensures no updates are 
        // missed and/or received multiple times between init and subsequent monitoring.
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
            await base.OnDeactivateAsync(reason, cancellationToken);
        }
    }

    private async Task MonitorUpdates(ConfiguredCancelableAsyncEnumerable<T>.Enumerator enumerator)
    {
        // Demo: wait a while to show that no updates will be missed. They are stored
        // per subscriber on the pushing grain, waiting to be picked up by the async iterator.
        await Task.Delay(TimeSpan.FromSeconds(3));

        try
        {
            while (await enumerator.MoveNextAsync())
            {
                logger.LogInformation("{GrainId} received update: {Value}", this.GetGrainId(), enumerator.Current);
                _current = CreateCacheItem(enumerator.Current);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException && ex is not TaskCanceledException)
        {
            logger.LogError(ex, "Error receiving updates.");
            DeactivateOnIdle();
        }
        finally
        {
            await enumerator.DisposeAsync();
        }
    }

    public ValueTask<T> GetCurrentValue() => ValueTask.FromResult(_current.Value);

    public ValueTask Abort()
    {
        DeactivateOnIdle();
        return ValueTask.CompletedTask;
    }

    protected abstract IAsyncEnumerable<T> GetUpdates();

    private static CacheItem CreateCacheItem(T value) => new(value, DateTime.UtcNow);

    private record CacheItem(T Value, DateTime LastUpdated);
}
