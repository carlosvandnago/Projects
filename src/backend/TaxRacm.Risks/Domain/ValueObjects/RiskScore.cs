namespace TaxRacm.Risks.Domain.ValueObjects;

/// <summary>Encapsulates the Likelihood x Impact computation and label mappings.</summary>
public record RiskScore
{
    public int Likelihood { get; init; } // 1-5
    public int Impact { get; init; }     // 1-5
    public int Score => Likelihood * Impact; // 1-25

    private RiskScore() { }

    public static RiskScore Create(int likelihood, int impact)
    {
        if (likelihood < 1 || likelihood > 5)
            throw new ArgumentOutOfRangeException(nameof(likelihood), "Likelihood must be 1-5.");
        if (impact < 1 || impact > 5)
            throw new ArgumentOutOfRangeException(nameof(impact), "Impact must be 1-5.");
        return new RiskScore { Likelihood = likelihood, Impact = impact };
    }

    public string LikelihoodLabel => Likelihood switch
    {
        1 => "Rare",
        2 => "Unlikely",
        3 => "Possible",
        4 => "Likely",
        5 => "Almost Certain",
        _ => throw new InvalidOperationException()
    };

    public string ImpactLabel => Impact switch
    {
        1 => "Negligible",
        2 => "Minor",
        3 => "Moderate",
        4 => "Major",
        5 => "Critical",
        _ => throw new InvalidOperationException()
    };
}
