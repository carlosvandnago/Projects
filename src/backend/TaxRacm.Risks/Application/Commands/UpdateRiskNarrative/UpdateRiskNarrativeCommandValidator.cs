using FluentValidation;

namespace TaxRacm.Risks.Application.Commands.UpdateRiskNarrative;

public class UpdateRiskNarrativeCommandValidator : AbstractValidator<UpdateRiskNarrativeCommand>
{
    public UpdateRiskNarrativeCommandValidator()
    {
        RuleFor(x => x.RacmEntryId).NotEmpty();
        RuleFor(x => x.Causes).NotNull();
        RuleFor(x => x.Consequences).NotNull();
        RuleFor(x => x.Notes).MaximumLength(2000);
    }
}
