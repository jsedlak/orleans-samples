using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Shared.Services;

public interface IAccountStatusService
{
    Task<int> GetOnlineCount();

    Task<IEnumerable<AccountStatusView>> GetStatuses();

    Task SetStatus(AccountStatusView accountStatusView);
}
