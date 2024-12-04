using Microsoft.EntityFrameworkCore;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;
using OrleansSamples.Patterns.SatellitePattern.Domain.ServiceModel;

namespace OrleansSamples.Patterns.SatellitePattern.Domain.Services;

public sealed class EfAccountStatusRepository : IAccountStatusReadRepository, IAccountStatusWriteRepository
{
    private readonly AccountDbContext _context;

    public EfAccountStatusRepository(AccountDbContext context)
    {
        _context = context;
    }

    public Task<int> GetOnlineCount()
    {
        return Task.FromResult(
            _context.Statuses.Count(m => m.IsOnline)
        );
    }

    public async Task<OnlineStatus?> GetStatus(Guid accountId)
    {
        return await _context.Statuses.FirstOrDefaultAsync(
            m => m.AccountId == accountId
        );
    }

    public Task<OnlineStatus[]> GetStatuses()
    {
        return Task.FromResult(_context.Statuses.ToArray());
    }

    public async Task Upsert(OnlineStatus status)
    {
        var exists = _context.Statuses.Any(m => m.AccountId == status.AccountId);

        if(exists)
        {
            _context.Attach(status);
            _context.Entry(status).State = EntityState.Modified;
        }
        else
        {
            _context.Statuses.Add(status);
        }

        await _context.SaveChangesAsync();
    }
}
