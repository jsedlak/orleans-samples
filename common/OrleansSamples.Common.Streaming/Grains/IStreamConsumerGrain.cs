using Orleans;

namespace OrleansSamples.Common.Grains;

public interface IStreamConsumerGrain : IGrainWithStringKey
{
    ValueTask<int> GetCount();
}
