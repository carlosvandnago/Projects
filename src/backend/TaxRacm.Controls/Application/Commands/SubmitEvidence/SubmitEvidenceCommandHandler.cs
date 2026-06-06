using MediatR;
using TaxRacm.Controls.Domain.Repositories;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Application.Commands.SubmitEvidence;

public class SubmitEvidenceCommandHandler : IRequestHandler<SubmitEvidenceCommand, Result>
{
    private readonly IControlRepository _controls;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitEvidenceCommandHandler(IControlRepository controls, IUnitOfWork unitOfWork)
    {
        _controls = controls;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SubmitEvidenceCommand request, CancellationToken cancellationToken)
    {
        var control = await _controls.GetByIdAsync(new ControlId(request.ControlId), cancellationToken);
        if (control is null) return Result.Failure("Control not found.");
        control.SubmitEvidence(request.FileName, request.FileUrl, new UserId(request.UploadedById), request.Notes);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
