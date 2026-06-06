using Microsoft.EntityFrameworkCore;
using TaxRacm.Clients.Domain.Entities;
using TaxRacm.SharedKernel.Application;

namespace TaxRacm.Clients.Infrastructure.Persistence;

public class ClientsDbContext : DbContext, IUnitOfWork
{
    public ClientsDbContext(DbContextOptions<ClientsDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<TaxEntity> TaxEntities => Set<TaxEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("clients");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientsDbContext).Assembly);
    }
}
