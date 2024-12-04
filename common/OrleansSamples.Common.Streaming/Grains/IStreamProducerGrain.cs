using Orleans;

namespace OrleansSamples.Common.Grains;

public interface IStreamProducerGrain : IGrainWithGuidKey
{
    Task StartProducing();
}
