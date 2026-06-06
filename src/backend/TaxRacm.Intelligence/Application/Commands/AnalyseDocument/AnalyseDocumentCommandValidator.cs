using FluentValidation;

namespace TaxRacm.Intelligence.Application.Commands.AnalyseDocument;

public class AnalyseDocumentCommandValidator : AbstractValidator<AnalyseDocumentCommand>
{
    public AnalyseDocumentCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.RequestedById).NotEmpty();
        RuleFor(x => x.TaxType).NotEmpty();
        RuleFor(x => x.EntityContext).NotEmpty();
        RuleFor(x => x.DocumentText).NotEmpty().MaximumLength(100_000);
    }
}
