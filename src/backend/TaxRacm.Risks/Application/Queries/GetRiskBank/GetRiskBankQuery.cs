using MediatR;
using TaxRacm.Risks.Application.DTOs;

namespace TaxRacm.Risks.Application.Queries.GetRiskBank;

public record GetRiskBankQuery(string? TaxType = null, string? Country = null, string? Industry = null) : IRequest<IReadOnlyList<RiskBankEntryDto>>;
