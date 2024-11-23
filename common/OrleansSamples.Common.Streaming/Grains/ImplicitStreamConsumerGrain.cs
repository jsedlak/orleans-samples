using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Streams;
using Orleans.Streams.Core;
using OrleansSamples.Common;

namespace OrleansSamples.Common.Grains;

[ImplicitStreamSubscription(StreamingConstants.StreamNamespace)]
public sealed class ImplicitStreamConsumerGrain : 
    Grain, 
    IStreamConsumerGrain, 
    IAsyncObserver<int>, 
    IStreamSubscriptionObserver
{
    private readonly ILogger<IStreamConsumerGrain> _logger;

    private int _readCount = 0;

    public ImplicitStreamConsumerGrain(ILogger<IStreamConsumerGrain> logger)
    {
        _logger = logger;
    }

    public ValueTask<int> GetCount() 
    {
        return new ValueTask<int>(_readCount);
    }

    public Task OnNextAsync(int item, StreamSequenceToken? token = null)
    {
        _logger.LogInformation($"[{nameof(ImplicitStreamConsumerGrain)}] OnNextAsync: item: {item}, token = {token}");
        _readCount = item;
        return Task.CompletedTask;
    }

    public Task OnCompletedAsync()
    {
        _logger.LogInformation($"[{nameof(ImplicitStreamConsumerGrain)}] OnCompletedAsync");
        return Task.CompletedTask;
    }

    public Task OnErrorAsync(Exception ex)
    {
        _logger.LogInformation($"[{nameof(ImplicitStreamConsumerGrain)}] OnErrorAsync: {{Exception}}", ex);
        return Task.CompletedTask;
    }

    public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        var handle = handleFactory.Create<int>();
        await handle.ResumeAsync(this);
    }
}