namespace TaxRacm.Clients.Domain.ValueObjects;

/// <summary>Strongly-typed identifier for a Client aggregate.</summary>
public record ClientId(Guid Value)
{
    public static ClientId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
