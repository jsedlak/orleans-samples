using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.Streams.Core;

namespace ImplicitStreams.Shared;

[ImplicitStreamSubscription(Constants.StreamNamespace)]
public sealed class ConsumerGrain : Grain, IConsumerGrain, IAsyncObserver<int>, IStreamSubscriptionObserver
{
    private readonly ILogger<IConsumerGrain> _logger;
    private int _readCount = 0;

    public ConsumerGrain(ILogger<IConsumerGrain> logger)
    {
        _logger = logger;
    }

    public ValueTask<int> GetCount()
    {
        return new ValueTask<int>(_readCount);
    }

    public Task OnNextAsync(int item, StreamSequenceToken? token = null)
    {
        _logger.LogInformation($"[{nameof(ConsumerGrain)}] OnNextAsync: item: {item}, token = {token}");
        _readCount = item;
        return Task.CompletedTask;
    }

    public Task OnCompletedAsync()
    {
        _logger.LogInformation($"[{nameof(ConsumerGrain)}] OnCompletedAsync");
        return Task.CompletedTask;
    }

    public Task OnErrorAsync(Exception ex)
    {
        _logger.LogInformation($"[{nameof(ConsumerGrain)}] OnErrorAsync: {{Exception}}", ex);
        return Task.CompletedTask;
    }

    public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        var handle = handleFactory.Create<int>();
        await handle.ResumeAsync(this);
    }
}