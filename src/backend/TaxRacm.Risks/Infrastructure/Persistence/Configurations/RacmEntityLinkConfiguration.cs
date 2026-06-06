using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Risks.Infrastructure.Persistence.Configurations;

public class RacmEntityLinkConfiguration : IEntityTypeConfiguration<RacmEntityLink>
{
    public void Configure(EntityTypeBuilder<RacmEntityLink> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.RacmEntryId).HasConversion(id => id.Value, value => new RacmEntryId(value));
        builder.Property(l => l.EntityId).HasConversion(id => id.Value, value => new EntityId(value));
        builder.Property(l => l.LocalOwnerId).HasConversion(
            id => id == null ? (Guid?)null : id.Value,
            value => value == null ? null : new UserId(value.Value));
        builder.ToTable("RacmEntityLinks");
    }
}
