using MediatR;

namespace TaxRacm.SharedKernel.Domain;

/// <summary>Marker interface for all domain events.</summary>
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
