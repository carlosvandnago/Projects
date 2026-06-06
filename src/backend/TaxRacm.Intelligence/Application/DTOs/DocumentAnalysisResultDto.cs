namespace TaxRacm.Intelligence.Application.DTOs;

public record DocumentAnalysisResultDto(
    Guid AiRequestId,
    List<SuggestedRiskDto> SuggestedRisks,
    int TokensUsed);

public record SuggestedRiskDto(
    string RiskName,
    string Description,
    List<string> Causes,
    List<string> Consequences,
    List<string> SuggestedPreventiveControls,
    List<string> SuggestedMitigatingControls,
    int DefaultGrossLikelihood,
    int DefaultGrossImpact,
    string TaxType);
