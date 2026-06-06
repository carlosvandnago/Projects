using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Intelligence.Application.Commands.AnalyseDocument;
using TaxRacm.Intelligence.Application.Commands.AssessControl;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/intelligence")]
public class IntelligenceController : ControllerBase
{
    private readonly IMediator _mediator;
    public IntelligenceController(IMediator mediator) => _mediator = mediator;

    [HttpPost("analyse-document")]
    public async Task<IActionResult> AnalyseDocument([FromBody] AnalyseDocumentCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpPost("assess-control")]
    public async Task<IActionResult> AssessControl([FromBody] AssessControlCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }
}
