using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Risks.Application.Queries.GetRiskBank;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/risk-bank")]
public class RiskBankController : ControllerBase
{
    private readonly IMediator _mediator;
    public RiskBankController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? taxType, [FromQuery] string? country, [FromQuery] string? industry, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRiskBankQuery(taxType, country, industry), ct);
        return Ok(result);
    }
}
