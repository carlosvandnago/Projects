namespace TaxRacm.Risks.Domain.ValueObjects;

public record RiskBankEntryId(Guid Value)
{
    public static RiskBankEntryId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
