namespace TaxRacm.Intelligence.Application.DTOs;

public record ControlAssessmentDto(
    Guid AiRequestId,
    string Rating,
    List<string> Strengths,
    List<string> Improvements,
    string RewrittenControl,
    List<string> AdditionalControlsSuggested,
    int TokensUsed);
