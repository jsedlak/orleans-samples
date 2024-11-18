using SatellitePattern.Shared.Domain;
using SatellitePattern.Shared.Services;

namespace SatellitePattern.Data;

public class EntityFrameworkAccountStatusService : IAccountStatusService
{
    private readonly ApplicationDbContext _context;

    public EntityFrameworkAccountStatusService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> GetOnlineCount()
    {
        return Task.FromResult(
            _context.AccountStatuses.Count(m => m.IsOnline)
        );
    }

    public Task<IEnumerable<AccountStatusView>> GetStatuses()
    {
        return Task.FromResult(
            (IEnumerable<AccountStatusView>)_context.AccountStatuses
        );
    }

    public async Task SetStatus(AccountStatusView accountStatusView)
    {
        var existing = _context.AccountStatuses.SingleOrDefault(x => x.AccountId == accountStatusView.AccountId);

        if(existing == null)
        {
            _context.AccountStatuses.Add(new AccountStatusView
            {
                AccountId = accountStatusView.AccountId,
                IsOnline = accountStatusView.IsOnline,
                Status = accountStatusView.Status
            });
        }
        else
        {
            existing.IsOnline = accountStatusView.IsOnline;
            existing.Status = accountStatusView.Status;
        }

        await _context.SaveChangesAsync();
    }
}
