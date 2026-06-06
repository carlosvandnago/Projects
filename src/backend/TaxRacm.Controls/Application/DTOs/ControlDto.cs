namespace TaxRacm.Controls.Application.DTOs;

public record ControlDto(
    Guid Id,
    Guid RacmEntryId,
    string Name,
    string Description,
    string ControlType,
    Guid OwnerId,
    string Frequency,
    DateTime? LastTested,
    DateTime? NextDue,
    string EvidenceStatus,
    bool RequiresReview,
    Guid? ReviewerId,
    string ReviewStatus,
    string Effectiveness,
    List<EvidenceDto> Evidence);

public record EvidenceDto(Guid Id, string FileName, string FileUrl, Guid UploadedById, string Notes, DateTime UploadedAt);
