using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Actors.EventDriven;

public interface IAccountStatusReporter : IGrainWithIntegerKey
{
    Task<int> GetOnlineCount();

    Task<AccountStatusView[]> GetAccountStatuses();
}
