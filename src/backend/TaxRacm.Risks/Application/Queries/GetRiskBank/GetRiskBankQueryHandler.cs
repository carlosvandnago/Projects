using MediatR;
using TaxRacm.Risks.Application.DTOs;
using TaxRacm.Risks.Domain.Enums;
using TaxRacm.Risks.Domain.Repositories;

namespace TaxRacm.Risks.Application.Queries.GetRiskBank;

public class GetRiskBankQueryHandler : IRequestHandler<GetRiskBankQuery, IReadOnlyList<RiskBankEntryDto>>
{
    private readonly IRiskBankRepository _riskBank;
    public GetRiskBankQueryHandler(IRiskBankRepository riskBank) => _riskBank = riskBank;

    public async Task<IReadOnlyList<RiskBankEntryDto>> Handle(GetRiskBankQuery request, CancellationToken cancellationToken)
    {
        TaxType? taxType = request.TaxType is not null && Enum.TryParse<TaxType>(request.TaxType, out var t) ? t : null;
        var entries = await _riskBank.GetFilteredAsync(taxType, request.Country, request.Industry, cancellationToken);
        return entries.Select(e => new RiskBankEntryDto(
            e.Id.Value, e.Name, e.TaxType.ToString(), e.Description,
            e.Causes, e.Consequences, e.SuggestedPreventiveControls, e.SuggestedMitigatingControls,
            e.ApplicableCountries, e.Industries, e.Tags,
            e.DefaultGrossLikelihood, e.DefaultGrossImpact, e.IsActive)).ToList();
    }
}
