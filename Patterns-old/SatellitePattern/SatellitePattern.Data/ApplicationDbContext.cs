using Microsoft.EntityFrameworkCore;
using SatellitePattern.Shared.Domain;

namespace SatellitePattern.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<AccountStatusView> AccountStatuses { get; set; }
}
