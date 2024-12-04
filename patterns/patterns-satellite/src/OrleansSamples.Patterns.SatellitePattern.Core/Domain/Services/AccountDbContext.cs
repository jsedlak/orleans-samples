using Microsoft.EntityFrameworkCore;
using OrleansSamples.Patterns.SatellitePattern.Domain.Model;

namespace OrleansSamples.Patterns.SatellitePattern.Domain.Services;

public sealed class AccountDbContext : DbContext
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OnlineStatus>().HasKey(m => m.AccountId);
    }

    public DbSet<OnlineStatus> Statuses { get; set; } = null!;
}