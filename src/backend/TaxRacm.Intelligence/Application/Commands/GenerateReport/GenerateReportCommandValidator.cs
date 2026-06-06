using FluentValidation;

namespace TaxRacm.Intelligence.Application.Commands.GenerateReport;

public class GenerateReportCommandValidator : AbstractValidator<GenerateReportCommand>
{
    public GenerateReportCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.RequestedById).NotEmpty();
        RuleFor(x => x.ClientName).NotEmpty();
        RuleFor(x => x.ReportType).NotEmpty();
        RuleFor(x => x.RacmEntries).NotEmpty();
    }
}
