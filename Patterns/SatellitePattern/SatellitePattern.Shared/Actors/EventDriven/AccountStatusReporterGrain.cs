using Orleans.Concurrency;
using SatellitePattern.Shared.Domain;
using SatellitePattern.Shared.Services;

namespace SatellitePattern.Shared.Actors.EventDriven;

[StatelessWorker(maxLocalWorkers: 1)]
public class AccountStatusReporterGrain : Grain, IAccountStatusReporter
{
    private readonly IAccountStatusService _accountStatusService;

    public AccountStatusReporterGrain(IAccountStatusService accountStatusService)
    {
        _accountStatusService = accountStatusService;
    }

    public async Task<AccountStatusView[]> GetAccountStatuses()
    {
        return (await _accountStatusService.GetStatuses()).ToArray();
    }

    public async Task<int> GetOnlineCount()
    {
        return await _accountStatusService.GetOnlineCount();
    }
}
