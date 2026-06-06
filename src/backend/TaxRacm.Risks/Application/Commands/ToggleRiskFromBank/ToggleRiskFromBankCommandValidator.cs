using FluentValidation;

namespace TaxRacm.Risks.Application.Commands.ToggleRiskFromBank;

public class ToggleRiskFromBankCommandValidator : AbstractValidator<ToggleRiskFromBankCommand>
{
    public ToggleRiskFromBankCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.RiskBankEntryId).NotEmpty();
        RuleFor(x => x.GlobalOwnerId).NotEmpty();
        RuleFor(x => x.GrossLikelihood).InclusiveBetween(1, 5);
        RuleFor(x => x.GrossImpact).InclusiveBetween(1, 5);
    }
}
