using Microsoft.VisualBasic;

namespace ImplicitStreams.Shared;

public interface IProducerGrain : IGrainWithGuidKey
{
    Task StartProducing();
}