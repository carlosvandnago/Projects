using MediatR;
using TaxRacm.Risks.Application.DTOs;

namespace TaxRacm.Risks.Application.Queries.GetRacmByClient;

public record GetRacmByClientQuery(Guid ClientId, string? TaxType = null, string? Rating = null) : IRequest<IReadOnlyList<RacmEntryDto>>;
