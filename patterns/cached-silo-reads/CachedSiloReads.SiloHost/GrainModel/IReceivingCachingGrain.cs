using Orleans.Concurrency;

namespace CachedSiloReads.SiloHost.GrainModel;

/// <summary>
/// Monitor and cache the latest value of <see cref="TCachedItem"/> on the local server.
/// </summary>
public interface IReceivingCachingGrain<TCachedItem> : IGrainWithStringKey where TCachedItem : notnull
{
    [ReadOnly]
    ValueTask<TCachedItem> GetCurrentValue();

    ValueTask Abort();
}
