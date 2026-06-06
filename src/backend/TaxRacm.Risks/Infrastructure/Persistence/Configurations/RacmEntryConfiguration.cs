using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Risks.Infrastructure.Persistence.Configurations;

public class RacmEntryConfiguration : IEntityTypeConfiguration<RacmEntry>
{
    public void Configure(EntityTypeBuilder<RacmEntry> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasConversion(id => id.Value, value => new RacmEntryId(value));
        builder.Property(e => e.ClientId).HasConversion(id => id.Value, value => new ClientId(value));
        builder.Property(e => e.RiskBankEntryId).HasConversion(
            id => id == null ? (Guid?)null : id.Value,
            value => value == null ? null : new RiskBankEntryId(value.Value));
        builder.Property(e => e.GlobalOwnerId).HasConversion(id => id.Value, value => new UserId(value));
        builder.OwnsOne(e => e.GrossScore, s => {
            s.Property(x => x.Likelihood).HasColumnName("GrossLikelihood");
            s.Property(x => x.Impact).HasColumnName("GrossImpact");
        });
        builder.OwnsOne(e => e.NetScore, s => {
            s.Property(x => x.Likelihood).HasColumnName("NetLikelihood");
            s.Property(x => x.Impact).HasColumnName("NetImpact");
        });
        builder.Property(e => e.Causes).HasConversion(
            l => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null),
            j => JsonSerializer.Deserialize<List<string>>(j, (JsonSerializerOptions?)null) ?? new());
        builder.Property(e => e.Consequences).HasConversion(
            l => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null),
            j => JsonSerializer.Deserialize<List<string>>(j, (JsonSerializerOptions?)null) ?? new());
        builder.HasMany(e => e.LinkedEntities).WithOne().HasForeignKey(l => l.RacmEntryId);
        builder.Navigation(e => e.LinkedEntities).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(e => e.Name).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Notes).HasMaxLength(2000);
        builder.ToTable("RacmEntries");
    }
}
