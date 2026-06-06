using FluentValidation;

namespace TaxRacm.Controls.Application.Commands.ReviewControl;

public class ReviewControlCommandValidator : AbstractValidator<ReviewControlCommand>
{
    public ReviewControlCommandValidator()
    {
        RuleFor(x => x.ControlId).NotEmpty();
        RuleFor(x => x.ReviewerId).NotEmpty();
        RuleFor(x => x.Decision).NotEmpty();
    }
}
