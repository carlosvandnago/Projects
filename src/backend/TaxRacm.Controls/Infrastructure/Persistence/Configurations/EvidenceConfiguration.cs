using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Controls.Domain.Entities;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Controls.Infrastructure.Persistence.Configurations;

public class EvidenceConfiguration : IEntityTypeConfiguration<Evidence>
{
    public void Configure(EntityTypeBuilder<Evidence> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ControlId)
            .HasConversion(id => id.Value, value => new ControlId(value));
        builder.Property(e => e.UploadedById)
            .HasConversion(id => id.Value, value => new UserId(value));
        builder.Property(e => e.FileName).HasMaxLength(500);
        builder.Property(e => e.FileUrl).HasMaxLength(1000);
        builder.Property(e => e.Notes).HasMaxLength(2000);
        builder.ToTable("Evidence");
    }
}
