using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Intelligence.Application.Commands.GenerateReport;
using TaxRacm.Intelligence.Application.DTOs;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReportsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("{clientId:guid}")]
    public async Task<IActionResult> Generate(Guid clientId, [FromBody] GenerateReportApiRequest request, CancellationToken ct)
    {
        var command = new GenerateReportCommand(clientId, request.RequestedById, request.ClientName, request.ReportType, request.EntityFilter, request.RacmEntries);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }
}

public record GenerateReportApiRequest(Guid RequestedById, string ClientName, string ReportType, string? EntityFilter, List<RacmSummaryDto> RacmEntries);
