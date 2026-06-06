using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.UpdateRiskNarrative;

public record UpdateRiskNarrativeCommand(Guid RacmEntryId, List<string> Causes, List<string> Consequences, string Notes) : IRequest<Result>;
