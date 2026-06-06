using MediatR;
using TaxRacm.Controls.Application.DTOs;

namespace TaxRacm.Controls.Application.Queries.GetControlsByRisk;

public record GetControlsByRiskQuery(Guid RacmEntryId) : IRequest<IReadOnlyList<ControlDto>>;
