using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Orleans.Streams.Core;
using SatellitePattern.Shared.Domain;
using SatellitePattern.Shared.Services;

namespace SatellitePattern.Shared.Actors.EventDriven;

[ImplicitStreamSubscription(Constants.StreamNamespace)]
public sealed class AccountEventHandlerActor : Grain<OnlineStatus>, IAccountEventHandlerActor, IAsyncObserver<OnlineStatusSetEvent>, IStreamSubscriptionObserver
{
    private readonly ILogger<IAccountEventHandlerActor> _logger;
    private readonly IAccountStatusService _accountStatusService;

    public AccountEventHandlerActor(ILogger<IAccountEventHandlerActor> logger, IAccountStatusService accountStatusService)
    {
        _logger = logger;
        _accountStatusService = accountStatusService;
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

    public async Task OnNextAsync(OnlineStatusSetEvent item, StreamSequenceToken? token = null)
    {
        _logger.LogInformation($"Received online status event for account {item.AccountId}: {item.Status}");
        
        await _accountStatusService.SetStatus(new AccountStatusView
        {
            AccountId = item.AccountId,
            IsOnline = item.Status != null,
            Status = item.Status == null ? "Offline": $"Last seen {DateTime.UtcNow.ToShortTimeString()}, {item.Status}"
        });
    }

    public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        var handle = handleFactory.Create<OnlineStatusSetEvent>();
        await handle.ResumeAsync(this);
    }
}