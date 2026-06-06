namespace TaxRacm.Risks.Application.DTOs;

public record RiskBankEntryDto(
    Guid Id,
    string Name,
    string TaxType,
    string Description,
    List<string> Causes,
    List<string> Consequences,
    List<string> SuggestedPreventiveControls,
    List<string> SuggestedMitigatingControls,
    List<string> ApplicableCountries,
    List<string> Industries,
    List<string> Tags,
    int DefaultGrossLikelihood,
    int DefaultGrossImpact,
    bool IsActive);
