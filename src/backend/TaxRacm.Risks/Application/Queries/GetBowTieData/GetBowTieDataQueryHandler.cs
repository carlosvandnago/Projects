using MediatR;
using TaxRacm.Risks.Application.DTOs;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Queries.GetBowTieData;

public class GetBowTieDataQueryHandler : IRequestHandler<GetBowTieDataQuery, Result<BowTieDto>>
{
    private readonly IRacmRepository _racm;
    public GetBowTieDataQueryHandler(IRacmRepository racm) => _racm = racm;

    public async Task<Result<BowTieDto>> Handle(GetBowTieDataQuery request, CancellationToken cancellationToken)
    {
        var entry = await _racm.GetByIdAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        if (entry is null) return Result<BowTieDto>.Failure("RACM entry not found.");
        return Result<BowTieDto>.Success(new BowTieDto(
            entry.Id.Value, entry.Name,
            entry.GrossScore.Score, entry.GrossRiskRating.Label, entry.GrossRiskRating.Colour,
            entry.NetScore.Score, entry.NetRiskRating.Label, entry.NetRiskRating.Colour,
            entry.Causes, entry.Consequences,
            new List<BowTieControlDto>(), new List<BowTieControlDto>()));
    }
}
