using MediatR;
using TaxRacm.Risks.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Queries.GetBowTieData;

public record GetBowTieDataQuery(Guid ClientId, Guid RacmEntryId) : IRequest<Result<BowTieDto>>;
