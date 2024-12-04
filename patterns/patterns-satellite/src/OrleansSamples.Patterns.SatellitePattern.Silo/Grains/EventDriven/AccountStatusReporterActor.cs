using Orleans.Concurrency;
using Orleans.Streams;
using Orleans.Streams.Core;
using OrleansSamples.Patterns.SatellitePattern.Domain.Events;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;

namespace OrleansSamples.Patterns.SatellitePattern.Silo.Grains.EventDriven;

[StatelessWorker(maxLocalWorkers: 1)]

public sealed class AccountStatusReporterActor : Grain, 
    IAccountStatusReporterActor
{
    private Dictionary<Guid, OnlineStatus> _statuses = new();

    public Task SetStatus(OnlineStatus status)
    {
        _statuses[status.AccountId] = status;
        return Task.CompletedTask;
    }

    public Task<int> GetOnlineCount()
    {
        return Task.FromResult(
            _statuses.Values.Count(m => m.IsOnline)
        );
    }

    public Task<OnlineStatus> GetStatus(Guid accountId)
    {
        _statuses.TryGetValue(accountId, out var status);

        return Task.FromResult(
            status ?? new()
        );
    }

    public Task<OnlineStatus[]> GetStatuses()
    {
        return Task.FromResult(
            _statuses.Values.ToArray()
        );
    }
}