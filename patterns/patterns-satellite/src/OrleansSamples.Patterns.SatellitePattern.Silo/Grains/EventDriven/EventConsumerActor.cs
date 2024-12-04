using Orleans.Streams.Core;
using Orleans.Streams;
using OrleansSamples.Patterns.SatellitePattern.Domain.Events;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.EventDriven;

[ImplicitStreamSubscription(EventDrivenConstants.StreamNamespace)]
public class EventConsumerActor : Grain, IEventConsumerActor,
    IAsyncObserver<OnlineStatusSetEvent>, IStreamSubscriptionObserver
{
    private readonly ILogger<IAccountStatusReporterActor> _logger;

    public EventConsumerActor(ILogger<IAccountStatusReporterActor> logger)
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

    public async Task OnNextAsync(OnlineStatusSetEvent item, StreamSequenceToken? token = null)
    {
        _logger.LogInformation($"Received online status event for account {item.AccountId}: {item.Status}");

        var reporter = GrainFactory.GetGrain<IAccountStatusReporterActor>(0);
        await reporter.SetStatus(new OnlineStatus
        {
            AccountId = item.AccountId,
            IsOnline = item.Status != null,
            Status = item.Status ?? "Offline",
            LastSeen = DateTime.UtcNow
        });
    }

    public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        var handle = handleFactory.Create<OnlineStatusSetEvent>();
        await handle.ResumeAsync(this);
    }
}
