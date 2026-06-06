using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Application.Commands.ReviewControl;

public record ReviewControlCommand(Guid ControlId, Guid ReviewerId, string Decision, string? Comments) : IRequest<Result>;
