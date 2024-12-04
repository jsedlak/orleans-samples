using OrleansSamples.Patterns.SatellitePattern.Domain.Model;

namespace OrleansSamples.Patterns.SatellitePattern.Domain.ServiceModel;

public interface IAccountStatusReadRepository
{
    Task<int> GetOnlineCount();

    Task<OnlineStatus[]> GetStatuses();

    Task<OnlineStatus?> GetStatus(Guid accountId);
}
