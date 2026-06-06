using FluentValidation;

namespace TaxRacm.Controls.Application.Commands.AddControl;

public class AddControlCommandValidator : AbstractValidator<AddControlCommand>
{
    public AddControlCommandValidator()
    {
        RuleFor(x => x.RacmEntryId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.ControlType).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
        RuleFor(x => x.Frequency).NotEmpty();
    }
}
