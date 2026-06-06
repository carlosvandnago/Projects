using TaxRacm.Controls.Domain.Enums;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Domain.Entities;

/// <summary>Aggregate root representing an internal control that mitigates a RACM risk.</summary>
public class Control : AggregateRoot<ControlId>
{
    public RacmEntryId RacmEntryId { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ControlType ControlType { get; private set; }
    public UserId OwnerId { get; private set; } = null!;
    public ControlFrequency Frequency { get; private set; }
    public DateTime? LastTested { get; private set; }
    public DateTime? NextDue { get; private set; }
    public EvidenceStatus EvidenceStatus { get; private set; }
    public bool RequiresReview { get; private set; }
    public UserId? ReviewerId { get; private set; }
    public ReviewStatus ReviewStatus { get; private set; }
    public ControlEffectiveness Effectiveness { get; private set; }

    private readonly List<Evidence> _evidence = new();
    public IReadOnlyCollection<Evidence> Evidence => _evidence.AsReadOnly();

    private Control() { }

    public static Control Create(
        RacmEntryId racmEntryId,
        string name,
        string description,
        ControlType controlType,
        UserId ownerId,
        ControlFrequency frequency) => new()
    {
        Id = ControlId.New(),
        RacmEntryId = racmEntryId,
        Name = name,
        Description = description,
        ControlType = controlType,
        OwnerId = ownerId,
        Frequency = frequency,
        EvidenceStatus = EvidenceStatus.NotStarted,
        ReviewStatus = ReviewStatus.NotRequired,
        Effectiveness = ControlEffectiveness.NotAssessed
    };

    public void Update(string name, string description, ControlType controlType, ControlFrequency frequency)
    {
        Name = name;
        Description = description;
        ControlType = controlType;
        Frequency = frequency;
    }

    public void SubmitEvidence(string fileName, string fileUrl, UserId uploadedBy, string notes)
    {
        var evidence = Entities.Evidence.Create(Id, fileName, fileUrl, uploadedBy, notes);
        _evidence.Add(evidence);
        EvidenceStatus = EvidenceStatus.Evidenced;
        LastTested = DateTime.UtcNow;
        RecalculateNextDue();
    }

    public Result Review(UserId reviewerId, ReviewStatus decision, string? comments = null)
    {
        if (!RequiresReview) return Result.Failure("This control does not require a review.");
        if (ReviewerId != reviewerId) return Result.Failure("Only the assigned reviewer can review this control.");
        ReviewStatus = decision;
        return Result.Success();
    }

    public void UpdateEffectiveness(ControlEffectiveness effectiveness) => Effectiveness = effectiveness;

    public void SetReviewRequirement(bool required, UserId? reviewerId)
    {
        RequiresReview = required;
        ReviewerId = reviewerId;
        ReviewStatus = required ? ReviewStatus.Pending : ReviewStatus.NotRequired;
    }

    private void RecalculateNextDue()
    {
        NextDue = Frequency switch
        {
            ControlFrequency.Monthly => LastTested?.AddMonths(1),
            ControlFrequency.Quarterly => LastTested?.AddMonths(3),
            ControlFrequency.Annual => LastTested?.AddYears(1),
            _ => null
        };
    }
}
