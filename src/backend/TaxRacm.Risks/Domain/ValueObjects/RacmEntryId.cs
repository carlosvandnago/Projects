namespace TaxRacm.Risks.Domain.ValueObjects;

public record RacmEntryId(Guid Value)
{
    public static RacmEntryId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
