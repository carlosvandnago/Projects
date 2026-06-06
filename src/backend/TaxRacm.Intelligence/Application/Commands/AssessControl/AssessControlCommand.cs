using MediatR;
using TaxRacm.Intelligence.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Intelligence.Application.Commands.AssessControl;

public record AssessControlCommand(Guid ClientId, Guid RequestedById, string RiskName, int GrossScore, int NetScore, List<string> ExistingControls, string ControlToAssess) : IRequest<Result<ControlAssessmentDto>>;
