using Orleans.Streams;
using Orleans.Streams.Core;
using OrleansSamples.Patterns.SatellitePattern.Domain.Events;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Domain.ServiceModel;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDrivenDatabase;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.EventDrivenDatabase;

[ImplicitStreamSubscription(EventDrivenDbConstants.StreamNamespace)]
public class EventConsumerDatabaseActor : Grain, IEventConsumerDatabaseActor,
    IAsyncObserver<OnlineStatusSetEvent>, IStreamSubscriptionObserver
{
    private readonly IAccountStatusWriteRepository _statusWriteRepository;
    private readonly ILogger<IAccountStatusReporterActor> _logger;

    public EventConsumerDatabaseActor(
        IAccountStatusWriteRepository statusWriteRepository,    
        ILogger<IAccountStatusReporterActor> logger)
    {
        _statusWriteRepository = statusWriteRepository;
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

        await _statusWriteRepository.Upsert(new OnlineStatus
        {
            AccountId = item.AccountId,
            IsOnline = item.Status != null,
            Status = item.Status ?? "Offline",
            LastSeen = DateTime.Now
        });
    }

    public async Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory)
    {
        var handle = handleFactory.Create<OnlineStatusSetEvent>();
        await handle.ResumeAsync(this);
    }
}
