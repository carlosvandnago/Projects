using TaxRacm.Controls.Domain.ValueObjects;

namespace TaxRacm.Controls.Domain.Services;

/// <summary>Domain service that derives review requirements from the net risk score. Thresholds are configurable.</summary>
public class ReviewThresholdService
{
    private const int MandatoryReviewThreshold = 15;
    private const int SelfCertifyWithVisibilityThreshold = 9;

    public ReviewRequirement DetermineRequirement(int netScore) => netScore switch
    {
        >= MandatoryReviewThreshold => new(true, "Mandatory — named KPMG or global reviewer required."),
        >= SelfCertifyWithVisibilityThreshold => new(false, "Self-certify — visible to KPMG and global owner."),
        _ => new(false, "Self-certify.")
    };
}
