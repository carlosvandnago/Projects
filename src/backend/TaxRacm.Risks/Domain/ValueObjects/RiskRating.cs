namespace TaxRacm.Risks.Domain.ValueObjects;

/// <summary>Qualitative risk rating derived from the numeric L×I score.</summary>
public record RiskRating(string Label, string Colour)
{
    public static RiskRating FromScore(int score) => score switch
    {
        >= 20 => new("Critical", "#d0021b"),
        >= 15 => new("High",     "#f5a623"),
        >= 9  => new("Medium",   "#f5cd00"),
        >= 5  => new("Low",      "#85bb2f"),
        _     => new("Minimal",  "#417505"),
    };
}
