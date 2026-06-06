using MediatR;
using TaxRacm.Controls.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Application.Commands.AddControl;

public record AddControlCommand(
    Guid RacmEntryId,
    string Name,
    string Description,
    string ControlType,
    Guid OwnerId,
    string Frequency) : IRequest<Result<ControlDto>>;
