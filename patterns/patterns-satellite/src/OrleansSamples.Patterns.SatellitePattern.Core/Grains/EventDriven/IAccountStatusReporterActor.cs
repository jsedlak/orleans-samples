using OrleansSamples.Patterns.SatellitePattern.Domain.Model;

namespace OrleansSamples.Patterns.SatellitePattern.Grains.EventDriven;

public interface IAccountStatusReporterActor : IGrainWithIntegerKey
{
    Task<int> GetOnlineCount();

    Task<OnlineStatus[]> GetStatuses();

    Task<OnlineStatus> GetStatus(Guid accountId);

    Task SetStatus(OnlineStatus status);
}