using MediatR;
using TaxRacm.Controls.Application.DTOs;

namespace TaxRacm.Controls.Application.Queries.GetUpcomingDeadlines;

public record GetUpcomingDeadlinesQuery(Guid ClientId, int DaysAhead = 30) : IRequest<IReadOnlyList<ControlDto>>;
