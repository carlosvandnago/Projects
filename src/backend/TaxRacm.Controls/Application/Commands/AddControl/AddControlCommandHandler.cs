using MediatR;
using TaxRacm.Controls.Application.DTOs;
using TaxRacm.Controls.Domain.Enums;
using TaxRacm.Controls.Domain.Entities;
using TaxRacm.Controls.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Application.Commands.AddControl;

public class AddControlCommandHandler : IRequestHandler<AddControlCommand, Result<ControlDto>>
{
    private readonly IControlRepository _controls;
    private readonly IUnitOfWork _unitOfWork;

    public AddControlCommandHandler(IControlRepository controls, IUnitOfWork unitOfWork)
    {
        _controls = controls;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ControlDto>> Handle(AddControlCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ControlType>(request.ControlType, out var controlType))
            return Result<ControlDto>.Failure($"Invalid ControlType: {request.ControlType}");
        if (!Enum.TryParse<ControlFrequency>(request.Frequency, out var frequency))
            return Result<ControlDto>.Failure($"Invalid Frequency: {request.Frequency}");

        var control = Control.Create(
            new RacmEntryId(request.RacmEntryId),
            request.Name, request.Description, controlType,
            new UserId(request.OwnerId), frequency);

        _controls.Add(control);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ControlDto>.Success(new ControlDto(
            control.Id.Value, control.RacmEntryId.Value,
            control.Name, control.Description, control.ControlType.ToString(),
            control.OwnerId.Value, control.Frequency.ToString(),
            control.LastTested, control.NextDue, control.EvidenceStatus.ToString(),
            control.RequiresReview, control.ReviewerId?.Value, control.ReviewStatus.ToString(),
            control.Effectiveness.ToString(), new List<EvidenceDto>()));
    }
}
