using FluentValidation;

namespace TaxRacm.Intelligence.Application.Commands.AssessControl;

public class AssessControlCommandValidator : AbstractValidator<AssessControlCommand>
{
    public AssessControlCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.RequestedById).NotEmpty();
        RuleFor(x => x.RiskName).NotEmpty();
        RuleFor(x => x.ControlToAssess).NotEmpty().MaximumLength(2000);
    }
}
