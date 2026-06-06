using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Controls.Application.Commands.AddControl;
using TaxRacm.Controls.Application.Commands.ReviewControl;
using TaxRacm.Controls.Application.Commands.SubmitEvidence;
using TaxRacm.Controls.Application.Queries.GetControlsByRisk;
using TaxRacm.Controls.Application.Queries.GetOverdueControls;
using TaxRacm.Controls.Application.Queries.GetUpcomingDeadlines;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/controls")]
public class ControlsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ControlsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("risk/{racmEntryId:guid}")]
    public async Task<IActionResult> GetByRisk(Guid racmEntryId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetControlsByRiskQuery(racmEntryId), ct);
        return Ok(result);
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue([FromQuery] Guid clientId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOverdueControlsQuery(clientId), ct);
        return Ok(result);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] Guid clientId, [FromQuery] int days = 30, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetUpcomingDeadlinesQuery(clientId, days), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddControlCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpPost("{controlId:guid}/evidence")]
    public async Task<IActionResult> SubmitEvidence(Guid controlId, [FromBody] SubmitEvidenceRequest request, CancellationToken ct)
    {
        var command = new SubmitEvidenceCommand(controlId, request.FileName, request.FileUrl, request.UploadedById, request.Notes);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok() : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpPost("{controlId:guid}/review")]
    public async Task<IActionResult> Review(Guid controlId, [FromBody] ReviewControlRequest request, CancellationToken ct)
    {
        var command = new ReviewControlCommand(controlId, request.ReviewerId, request.Decision, request.Comments);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok() : BadRequest(new ProblemDetails { Title = result.Error });
    }
}

public record SubmitEvidenceRequest(string FileName, string FileUrl, Guid UploadedById, string Notes);
public record ReviewControlRequest(Guid ReviewerId, string Decision, string? Comments);
