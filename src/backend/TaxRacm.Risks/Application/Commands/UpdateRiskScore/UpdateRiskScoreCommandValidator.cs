using FluentValidation;

namespace TaxRacm.Risks.Application.Commands.UpdateRiskScore;

public class UpdateRiskScoreCommandValidator : AbstractValidator<UpdateRiskScoreCommand>
{
    public UpdateRiskScoreCommandValidator()
    {
        RuleFor(x => x.RacmEntryId).NotEmpty();
        RuleFor(x => x.NetLikelihood).InclusiveBetween(1, 5);
        RuleFor(x => x.NetImpact).InclusiveBetween(1, 5);
    }
}
