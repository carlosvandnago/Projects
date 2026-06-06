using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Risks.Application.Commands.LinkEntityToRisk;
using TaxRacm.Risks.Application.Commands.ToggleRiskFromBank;
using TaxRacm.Risks.Application.Commands.UnlinkEntityFromRisk;
using TaxRacm.Risks.Application.Commands.UpdateRiskNarrative;
using TaxRacm.Risks.Application.Commands.UpdateRiskScore;
using TaxRacm.Risks.Application.Queries.GetBowTieData;
using TaxRacm.Risks.Application.Queries.GetRacmByClient;
using TaxRacm.Risks.Application.Queries.GetRiskById;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/racm")]
public class RacmController : ControllerBase
{
    private readonly IMediator _mediator;
    public RacmController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetByClient(Guid clientId, [FromQuery] string? taxType, [FromQuery] string? rating, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRacmByClientQuery(clientId, taxType, rating), ct);
        return Ok(result);
    }

    [HttpGet("{clientId:guid}/{riskId:guid}")]
    public async Task<IActionResult> GetById(Guid clientId, Guid riskId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRiskByIdQuery(riskId, clientId), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    [HttpGet("{clientId:guid}/{riskId:guid}/bowtie")]
    public async Task<IActionResult> GetBowTie(Guid clientId, Guid riskId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBowTieDataQuery(riskId, clientId), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    [HttpPost("{clientId:guid}/toggle-bank-risk")]
    public async Task<IActionResult> ToggleBankRisk(Guid clientId, [FromBody] ToggleBankRiskRequest request, CancellationToken ct)
    {
        var command = new ToggleRiskFromBankCommand(clientId, request.RiskBankEntryId, request.RequestedById);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpPut("{clientId:guid}/{riskId:guid}/score")]
    public async Task<IActionResult> UpdateScore(Guid clientId, Guid riskId, [FromBody] UpdateRiskScoreRequest request, CancellationToken ct)
    {
        var command = new UpdateRiskScoreCommand(riskId, request.NetLikelihood, request.NetImpact);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpPut("{clientId:guid}/{riskId:guid}/narrative")]
    public async Task<IActionResult> UpdateNarrative(Guid clientId, Guid riskId, [FromBody] UpdateRiskNarrativeRequest request, CancellationToken ct)
    {
        var command = new UpdateRiskNarrativeCommand(riskId, request.Causes, request.Consequences, request.Notes);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpPost("{clientId:guid}/{riskId:guid}/link-entity")]
    public async Task<IActionResult> LinkEntity(Guid clientId, Guid riskId, [FromBody] LinkEntityRequest request, CancellationToken ct)
    {
        var command = new LinkEntityToRiskCommand(riskId, request.EntityId, request.LocalOwnerId);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok() : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpDelete("{clientId:guid}/{riskId:guid}/link-entity/{entityId:guid}")]
    public async Task<IActionResult> UnlinkEntity(Guid clientId, Guid riskId, Guid entityId, CancellationToken ct)
    {
        var command = new UnlinkEntityFromRiskCommand(riskId, entityId);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : BadRequest(new ProblemDetails { Title = result.Error });
    }
}

public record ToggleBankRiskRequest(Guid RiskBankEntryId, Guid RequestedById);
public record UpdateRiskScoreRequest(int NetLikelihood, int NetImpact);
public record UpdateRiskNarrativeRequest(List<string> Causes, List<string> Consequences, string Notes);
public record LinkEntityRequest(Guid EntityId, Guid? LocalOwnerId);
