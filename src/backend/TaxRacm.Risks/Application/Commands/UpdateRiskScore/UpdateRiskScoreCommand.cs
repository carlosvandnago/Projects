using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.UpdateRiskScore;

public record UpdateRiskScoreCommand(Guid RacmEntryId, int NetLikelihood, int NetImpact) : IRequest<Result>;
