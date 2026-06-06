using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Clients.Application.Commands.CreateClient;
using TaxRacm.Clients.Application.Commands.CreateEntity;
using TaxRacm.Clients.Application.Queries.GetClientById;
using TaxRacm.Clients.Application.Queries.GetClients;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetClientsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
            : BadRequest(new ProblemDetails { Title = result.Error });
    }

    [HttpGet("{id:guid}/entities")]
    public async Task<IActionResult> GetEntities(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetClientByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value!.Entities) : NotFound(new ProblemDetails { Title = result.Error });
    }

    [HttpPost("{id:guid}/entities")]
    public async Task<IActionResult> AddEntity(Guid id, [FromBody] CreateEntityRequest request, CancellationToken ct)
    {
        var command = new CreateEntityCommand(id, request.Name, request.Country, request.Jurisdiction, request.EntityType, request.Region);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }
}

public record CreateEntityRequest(string Name, string Country, string Jurisdiction, string EntityType, string Region);
