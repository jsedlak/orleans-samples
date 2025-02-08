using Orleans;

namespace OrleansSamples.Common.Grains;

public interface IStreamProducerGrain : IGrainWithStringKey
{
    Task StartProducing();
}
