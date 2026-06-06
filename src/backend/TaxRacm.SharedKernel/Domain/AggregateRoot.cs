namespace TaxRacm.SharedKernel.Domain;

/// <summary>Base class for aggregate roots — the consistency boundary in the domain.</summary>
public abstract class AggregateRoot<TId> : Entity<TId> { }
