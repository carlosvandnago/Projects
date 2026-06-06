using FluentValidation;

namespace TaxRacm.Controls.Application.Commands.SubmitEvidence;

public class SubmitEvidenceCommandValidator : AbstractValidator<SubmitEvidenceCommand>
{
    public SubmitEvidenceCommandValidator()
    {
        RuleFor(x => x.ControlId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty().MaximumLength(500);
        RuleFor(x => x.FileUrl).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.UploadedById).NotEmpty();
    }
}
