using MediatR;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.UpdateRiskScore;

public class UpdateRiskScoreCommandHandler : IRequestHandler<UpdateRiskScoreCommand, Result>
{
    private readonly IRacmRepository _racm;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRiskScoreCommandHandler(IRacmRepository racm, IUnitOfWork unitOfWork)
    {
        _racm = racm;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateRiskScoreCommand request, CancellationToken cancellationToken)
    {
        var entry = await _racm.GetByIdAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        if (entry is null) return Result.Failure("RACM entry not found.");
        var result = entry.UpdateNetScore(request.NetLikelihood, request.NetImpact);
        if (result.IsFailure) return result;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
