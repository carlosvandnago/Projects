using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Domain.Entities;

public class RacmEntityLink : Entity<Guid>
{
    public RacmEntryId RacmEntryId { get; private set; } = null!;
    public EntityId EntityId { get; private set; } = null!;
    public UserId? LocalOwnerId { get; private set; }
    public DateTime LinkedAt { get; private set; }

    private RacmEntityLink() { }

    public RacmEntityLink(RacmEntryId racmEntryId, EntityId entityId, UserId? localOwnerId)
    {
        Id = Guid.NewGuid();
        RacmEntryId = racmEntryId;
        EntityId = entityId;
        LocalOwnerId = localOwnerId;
        LinkedAt = DateTime.UtcNow;
    }
}
