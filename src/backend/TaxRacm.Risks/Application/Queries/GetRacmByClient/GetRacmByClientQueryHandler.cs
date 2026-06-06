using MediatR;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Application.DTOs;
using TaxRacm.Risks.Domain.Repositories;

namespace TaxRacm.Risks.Application.Queries.GetRacmByClient;

public class GetRacmByClientQueryHandler : IRequestHandler<GetRacmByClientQuery, IReadOnlyList<RacmEntryDto>>
{
    private readonly IRacmRepository _racm;
    public GetRacmByClientQueryHandler(IRacmRepository racm) => _racm = racm;

    public async Task<IReadOnlyList<RacmEntryDto>> Handle(GetRacmByClientQuery request, CancellationToken cancellationToken)
    {
        var entries = await _racm.GetByClientAsync(new ClientId(request.ClientId), cancellationToken);
        var result = entries.AsEnumerable();
        if (request.TaxType is not null)
            result = result.Where(e => e.TaxType.ToString() == request.TaxType);
        if (request.Rating is not null)
            result = result.Where(e => e.NetRiskRating.Label == request.Rating);
        return result.Select(MapToDto).ToList();
    }

    internal static RacmEntryDto MapToDto(Domain.Entities.RacmEntry e) => new(
        e.Id.Value, e.ClientId.Value, e.RiskBankEntryId?.Value,
        e.Name, e.Description, e.TaxType.ToString(), e.Scope.ToString(),
        e.GlobalOwnerId.Value,
        e.GrossScore.Likelihood, e.GrossScore.Impact, e.GrossScore.Score,
        e.GrossRiskRating.Label, e.GrossRiskRating.Colour,
        e.NetScore.Likelihood, e.NetScore.Impact, e.NetScore.Score,
        e.NetRiskRating.Label, e.NetRiskRating.Colour,
        e.ControlEffectivenessDelta,
        e.Causes, e.Consequences, e.Status.ToString(), e.Notes, e.LastReviewed,
        e.LinkedEntities.Select(l => new LinkedEntityDto(l.EntityId.Value, l.LocalOwnerId?.Value)).ToList());
}
