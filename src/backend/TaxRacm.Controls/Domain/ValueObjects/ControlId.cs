namespace TaxRacm.Controls.Domain.ValueObjects;

public record ControlId(Guid Value)
{
    public static ControlId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
