using FluentValidation;

namespace TaxRacm.Risks.Application.Commands.LinkEntityToRisk;

public class LinkEntityToRiskCommandValidator : AbstractValidator<LinkEntityToRiskCommand>
{
    public LinkEntityToRiskCommandValidator()
    {
        RuleFor(x => x.RacmEntryId).NotEmpty();
        RuleFor(x => x.EntityId).NotEmpty();
    }
}
