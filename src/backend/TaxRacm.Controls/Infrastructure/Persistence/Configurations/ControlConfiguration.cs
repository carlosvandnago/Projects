using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Controls.Domain.Entities;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Controls.Infrastructure.Persistence.Configurations;

public class ControlConfiguration : IEntityTypeConfiguration<Control>
{
    public void Configure(EntityTypeBuilder<Control> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => new ControlId(value));
        builder.Property(c => c.RacmEntryId)
            .HasConversion(id => id.Value, value => new RacmEntryId(value));
        builder.Property(c => c.OwnerId)
            .HasConversion(id => id.Value, value => new UserId(value));
        builder.Property(c => c.ReviewerId)
            .HasConversion(id => id == null ? (Guid?)null : id.Value, value => value == null ? null : new UserId(value.Value));
        builder.Property(c => c.Name).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(2000);
        builder.HasMany(c => c.Evidence)
            .WithOne()
            .HasForeignKey("ControlId")
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.Evidence).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.ToTable("Controls");
    }
}
