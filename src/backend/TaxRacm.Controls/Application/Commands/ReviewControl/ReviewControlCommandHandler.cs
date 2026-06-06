using MediatR;
using TaxRacm.Controls.Domain.Enums;
using TaxRacm.Controls.Domain.Repositories;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Application.Commands.ReviewControl;

public class ReviewControlCommandHandler : IRequestHandler<ReviewControlCommand, Result>
{
    private readonly IControlRepository _controls;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewControlCommandHandler(IControlRepository controls, IUnitOfWork unitOfWork)
    {
        _controls = controls;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ReviewControlCommand request, CancellationToken cancellationToken)
    {
        var control = await _controls.GetByIdAsync(new ControlId(request.ControlId), cancellationToken);
        if (control is null) return Result.Failure("Control not found.");
        if (!Enum.TryParse<ReviewStatus>(request.Decision, out var decision))
            return Result.Failure($"Invalid decision: {request.Decision}");
        var result = control.Review(new UserId(request.ReviewerId), decision, request.Comments);
        if (result.IsFailure) return result;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
