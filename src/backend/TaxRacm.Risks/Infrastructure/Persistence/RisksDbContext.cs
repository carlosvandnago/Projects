using Microsoft.EntityFrameworkCore;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.SharedKernel.Application;

namespace TaxRacm.Risks.Infrastructure.Persistence;

public class RisksDbContext : DbContext, IUnitOfWork
{
    public RisksDbContext(DbContextOptions<RisksDbContext> options) : base(options) { }

    public DbSet<RiskBankEntry> RiskBankEntries => Set<RiskBankEntry>();
    public DbSet<RacmEntry> RacmEntries => Set<RacmEntry>();
    public DbSet<RacmEntityLink> RacmEntityLinks => Set<RacmEntityLink>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("risks");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RisksDbContext).Assembly);
    }
}
