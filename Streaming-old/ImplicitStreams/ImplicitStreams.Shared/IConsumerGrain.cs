namespace ImplicitStreams.Shared;

public interface IConsumerGrain : IGrainWithGuidKey
{
    ValueTask<int> GetCount();
}