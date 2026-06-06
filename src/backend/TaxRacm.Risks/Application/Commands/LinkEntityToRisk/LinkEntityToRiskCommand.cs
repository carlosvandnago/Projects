using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.LinkEntityToRisk;

public record LinkEntityToRiskCommand(Guid RacmEntryId, Guid EntityId, Guid? LocalOwnerId) : IRequest<Result>;
