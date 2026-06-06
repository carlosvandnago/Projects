using MediatR;
using TaxRacm.Controls.Application.DTOs;

namespace TaxRacm.Controls.Application.Queries.GetOverdueControls;

public record GetOverdueControlsQuery(Guid ClientId) : IRequest<IReadOnlyList<ControlDto>>;
