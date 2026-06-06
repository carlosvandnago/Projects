using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaxRacm.Controls.Application.Queries.GetOverdueControls;
using TaxRacm.Controls.Application.Queries.GetUpcomingDeadlines;
using TaxRacm.Risks.Application.Queries.GetRacmByClient;

namespace TaxRacm.Api.Controllers;

[ApiController]
[Route("api/v1/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    public DashboardController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetDashboard(Guid clientId, CancellationToken ct)
    {
        var racmTask = _mediator.Send(new GetRacmByClientQuery(clientId, null, null), ct);
        var overdueTask = _mediator.Send(new GetOverdueControlsQuery(clientId), ct);
        var upcomingTask = _mediator.Send(new GetUpcomingDeadlinesQuery(clientId, 30), ct);

        await Task.WhenAll(racmTask, overdueTask, upcomingTask);

        var racm = await racmTask;
        var overdue = await overdueTask;
        var upcoming = await upcomingTask;

        var ratingBreakdown = racm
            .GroupBy(r => r.NetRating)
            .ToDictionary(g => g.Key, g => g.Count());

        var taxTypeBreakdown = racm
            .GroupBy(r => r.TaxType)
            .ToDictionary(g => g.Key, g => g.Count());

        return Ok(new
        {
            TotalRisks = racm.Count,
            RatingBreakdown = ratingBreakdown,
            TaxTypeBreakdown = taxTypeBreakdown,
            OverdueControls = overdue.Count,
            UpcomingDeadlines = upcoming.Count,
            TopRisks = racm.OrderByDescending(r => r.NetScore).Take(5)
        });
    }
}
