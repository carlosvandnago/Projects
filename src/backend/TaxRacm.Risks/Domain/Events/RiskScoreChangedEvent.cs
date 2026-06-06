using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Domain.Events;

public record RiskScoreChangedEvent(RacmEntryId RacmEntryId, RiskScore OldScore, RiskScore NewScore) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
