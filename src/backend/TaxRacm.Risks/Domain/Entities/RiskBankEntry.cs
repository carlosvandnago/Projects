using TaxRacm.Risks.Domain.Enums;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Domain.Entities;

/// <summary>An entry in the KPMG curated risk library available to all clients.</summary>
public class RiskBankEntry : AggregateRoot<RiskBankEntryId>
{
    public string Name { get; private set; } = string.Empty;
    public TaxType TaxType { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public List<string> Causes { get; private set; } = new();
    public List<string> Consequences { get; private set; } = new();
    public List<string> SuggestedPreventiveControls { get; private set; } = new();
    public List<string> SuggestedMitigatingControls { get; private set; } = new();
    public List<string> ApplicableCountries { get; private set; } = new();
    public List<string> Industries { get; private set; } = new();
    public List<string> Tags { get; private set; } = new();
    public int DefaultGrossLikelihood { get; private set; }
    public int DefaultGrossImpact { get; private set; }
    public bool IsActive { get; private set; } = true;

    private RiskBankEntry() { }

    public static RiskBankEntry Create(
        string name,
        TaxType taxType,
        string description,
        List<string> causes,
        List<string> consequences,
        List<string> preventiveControls,
        List<string> mitigatingControls,
        int defaultGrossLikelihood,
        int defaultGrossImpact,
        List<string>? applicableCountries = null,
        List<string>? industries = null,
        List<string>? tags = null) => new()
    {
        Id = RiskBankEntryId.New(),
        Name = name,
        TaxType = taxType,
        Description = description,
        Causes = causes,
        Consequences = consequences,
        SuggestedPreventiveControls = preventiveControls,
        SuggestedMitigatingControls = mitigatingControls,
        DefaultGrossLikelihood = defaultGrossLikelihood,
        DefaultGrossImpact = defaultGrossImpact,
        ApplicableCountries = applicableCountries ?? new(),
        Industries = industries ?? new(),
        Tags = tags ?? new(),
        IsActive = true
    };

    public void Update(string name, string description, int defaultGrossLikelihood, int defaultGrossImpact)
    {
        Name = name;
        Description = description;
        DefaultGrossLikelihood = defaultGrossLikelihood;
        DefaultGrossImpact = defaultGrossImpact;
    }

    public void Deactivate() => IsActive = false;
}
