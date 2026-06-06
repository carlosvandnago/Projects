using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Clients.Domain.Entities;
using TaxRacm.Clients.Domain.ValueObjects;

namespace TaxRacm.Clients.Infrastructure.Persistence.Configurations;

public class TaxEntityConfiguration : IEntityTypeConfiguration<TaxEntity>
{
    public void Configure(EntityTypeBuilder<TaxEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasConversion(id => id.Value, value => new EntityId(value));
        builder.Property(e => e.ClientId)
            .HasConversion(id => id.Value, value => new ClientId(value));
        builder.Property(e => e.Country)
            .HasConversion(c => c.Value, value => new CountryCode(value))
            .HasMaxLength(2);
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Jurisdiction).HasMaxLength(200).IsRequired();
        builder.ToTable("TaxEntities");
    }
}
