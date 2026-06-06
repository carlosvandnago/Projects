using MediatR;
using TaxRacm.Intelligence.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Intelligence.Application.Commands.GenerateReport;

public record GenerateReportCommand(Guid ClientId, Guid RequestedById, string ClientName, string ReportType, string? EntityFilter, List<RacmSummaryDto> RacmEntries) : IRequest<Result<ReportDto>>;
