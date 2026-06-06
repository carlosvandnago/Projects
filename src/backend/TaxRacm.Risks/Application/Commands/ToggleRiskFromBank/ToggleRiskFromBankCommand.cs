using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.ToggleRiskFromBank;

public record ToggleRiskFromBankCommand(
    Guid ClientId,
    Guid RiskBankEntryId,
    Guid GlobalOwnerId,
    int GrossLikelihood,
    int GrossImpact,
    List<Guid> LinkedEntityIds) : IRequest<Result<Guid>>;
