using Orleans.Concurrency;

namespace CachedSiloReads.SiloHost.GrainModel;

/// <summary>
/// Monitor and cache the latest value of <see cref="T"/>.
/// </summary>
public interface IMonitorGrain<T> : IGrainWithStringKey where T : notnull
{
    [ReadOnly]
    ValueTask<T> GetCurrentValue();

    ValueTask Abort();
}
