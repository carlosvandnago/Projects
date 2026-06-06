using Microsoft.EntityFrameworkCore;
using TaxRacm.Intelligence.Domain.Entities;
using TaxRacm.SharedKernel.Application;

namespace TaxRacm.Intelligence.Infrastructure.Persistence;

public class IntelligenceDbContext : DbContext, IUnitOfWork
{
    public IntelligenceDbContext(DbContextOptions<IntelligenceDbContext> options) : base(options) { }

    public DbSet<AiRequest> AiRequests => Set<AiRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("intelligence");
        var aiRequest = modelBuilder.Entity<AiRequest>();
        aiRequest.HasKey(a => a.Id);
        aiRequest.Property(a => a.RequestedById).HasConversion(id => id.Value, v => new TaxRacm.Risks.Domain.ValueObjects.UserId(v));
        aiRequest.Property(a => a.InputData).HasMaxLength(int.MaxValue);
        aiRequest.Property(a => a.OutputData).HasMaxLength(int.MaxValue);
        aiRequest.Property(a => a.ModelUsed).HasMaxLength(100);
        aiRequest.ToTable("AiRequests");
    }
}
