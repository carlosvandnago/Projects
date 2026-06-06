using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.UnlinkEntityFromRisk;

public record UnlinkEntityFromRiskCommand(Guid RacmEntryId, Guid EntityId) : IRequest<Result>;
