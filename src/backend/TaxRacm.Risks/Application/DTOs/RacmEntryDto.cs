namespace TaxRacm.Risks.Application.DTOs;

public record RacmEntryDto(
    Guid Id,
    Guid ClientId,
    Guid? RiskBankEntryId,
    string Name,
    string Description,
    string TaxType,
    string Scope,
    Guid GlobalOwnerId,
    int GrossLikelihood,
    int GrossImpact,
    int GrossScore,
    string GrossRating,
    string GrossRatingColour,
    int NetLikelihood,
    int NetImpact,
    int NetScore,
    string NetRating,
    string NetRatingColour,
    int ControlEffectivenessDelta,
    List<string> Causes,
    List<string> Consequences,
    string Status,
    string Notes,
    DateTime LastReviewed,
    List<LinkedEntityDto> LinkedEntities);

public record LinkedEntityDto(Guid EntityId, Guid? LocalOwnerId);
