using Orleans;

namespace OrleansSamples.Common.Grains;

public interface IStreamConsumerGrain : IGrainWithGuidKey
{
    ValueTask<int> GetCount();
}
