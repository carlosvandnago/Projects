using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Risks.Infrastructure.Persistence.Configurations;

public class RiskBankEntryConfiguration : IEntityTypeConfiguration<RiskBankEntry>
{
    public void Configure(EntityTypeBuilder<RiskBankEntry> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasConversion(id => id.Value, value => new RiskBankEntryId(value));
        builder.Property(e => e.Name).HasMaxLength(300).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(2000);
        foreach (var prop in new[] { "Causes","Consequences","SuggestedPreventiveControls","SuggestedMitigatingControls","ApplicableCountries","Industries","Tags" })
            builder.Property<List<string>>(prop).HasConversion(
                l => JsonSerializer.Serialize(l, (JsonSerializerOptions?)null),
                j => JsonSerializer.Deserialize<List<string>>(j, (JsonSerializerOptions?)null) ?? new());
        builder.ToTable("RiskBankEntries");
    }
}
