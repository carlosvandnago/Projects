namespace TaxRacm.Risks.Application.DTOs;

public record BowTieDto(
    Guid RacmEntryId,
    string RiskName,
    int GrossScore,
    string GrossRating,
    string GrossRatingColour,
    int NetScore,
    string NetRating,
    string NetRatingColour,
    List<string> Causes,
    List<string> Consequences,
    List<BowTieControlDto> PreventiveControls,
    List<BowTieControlDto> MitigatingControls);

public record BowTieControlDto(
    Guid Id,
    string Name,
    string Effectiveness,
    string? OwnerName,
    DateTime? NextDue,
    string EvidenceStatus);
