using Microsoft.EntityFrameworkCore;
using TaxRacm.Controls.Domain.Entities;
using TaxRacm.SharedKernel.Application;

namespace TaxRacm.Controls.Infrastructure.Persistence;

public class ControlsDbContext : DbContext, IUnitOfWork
{
    public ControlsDbContext(DbContextOptions<ControlsDbContext> options) : base(options) { }

    public DbSet<Control> Controls => Set<Control>();
    public DbSet<Evidence> Evidences => Set<Evidence>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("controls");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ControlsDbContext).Assembly);
    }
}
