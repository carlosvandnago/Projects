using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Clients.Domain.Entities;
using TaxRacm.Clients.Domain.ValueObjects;

namespace TaxRacm.Clients.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, value => new ClientId(value));
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Industry).HasMaxLength(100).IsRequired();
        builder.HasMany(c => c.Entities)
            .WithOne()
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(c => c.Entities).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.ToTable("Clients");
    }
}
