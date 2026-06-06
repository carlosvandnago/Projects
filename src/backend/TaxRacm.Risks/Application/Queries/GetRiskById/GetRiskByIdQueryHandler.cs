using MediatR;
using TaxRacm.Risks.Application.DTOs;
using TaxRacm.Risks.Application.Queries.GetRacmByClient;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Queries.GetRiskById;

public class GetRiskByIdQueryHandler : IRequestHandler<GetRiskByIdQuery, Result<RacmEntryDto>>
{
    private readonly IRacmRepository _racm;
    public GetRiskByIdQueryHandler(IRacmRepository racm) => _racm = racm;

    public async Task<Result<RacmEntryDto>> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
    {
        var entry = await _racm.GetByIdAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        if (entry is null) return Result<RacmEntryDto>.Failure("RACM entry not found.");
        return Result<RacmEntryDto>.Success(GetRacmByClientQueryHandler.MapToDto(entry));
    }
}
