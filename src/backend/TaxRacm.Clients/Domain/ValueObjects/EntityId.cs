namespace TaxRacm.Clients.Domain.ValueObjects;

/// <summary>Strongly-typed identifier for a TaxEntity.</summary>
public record EntityId(Guid Value)
{
    public static EntityId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
