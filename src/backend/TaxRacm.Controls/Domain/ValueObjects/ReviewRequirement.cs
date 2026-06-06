namespace TaxRacm.Controls.Domain.ValueObjects;

/// <summary>Computed from the net risk score — determines if a named reviewer is required.</summary>
public record ReviewRequirement(bool RequiresNamedReviewer, string Description);
