using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.Streams.Core;
using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors.EventDriven;

[ImplicitStreamSubscription(Constants.StreamNamespace)]
public sealed class AccountEventHandlerActor : Grain<OnlineStatus>, IAccountEventHandlerActor, IAsyncObserver<OnlineStatusSetEvent>, IStreamSubscriptionObserver
{
    private readonly ILogger<IAccountEventHandlerActor> _logger;

    public AccountEventHandlerActor(ILogger<IAccountEventHandlerActor> logger)
    {
        _logger = logger;
    }

    public Task OnCompletedAsync()
    {
        return Task.CompletedTask;
    }

    public Task OnErrorAsync(Exception ex)
    {
        _logger.LogError(ex, ex.Message);
        return Task.CompletedTask;
    }

    public Task OnNextAsync(OnlineStatusSetEvent item, StreamSequenceToken? token = null)
    {
        _logger.LogInformation($"Received online status event for account {item.AccountId}: {item.Status}");
        _logger.LogInformation("Pretending to store in a database...");

        return Task.CompletedTask;
    }

    public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        var handle = handleFactory.Create<OnlineStatusSetEvent>();
        await handle.ResumeAsync(this);
    }
}