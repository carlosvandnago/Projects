using MediatR;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.UpdateRiskNarrative;

public class UpdateRiskNarrativeCommandHandler : IRequestHandler<UpdateRiskNarrativeCommand, Result>
{
    private readonly IRacmRepository _racm;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRiskNarrativeCommandHandler(IRacmRepository racm, IUnitOfWork unitOfWork)
    {
        _racm = racm;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateRiskNarrativeCommand request, CancellationToken cancellationToken)
    {
        var entry = await _racm.GetByIdAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        if (entry is null) return Result.Failure("RACM entry not found.");
        entry.UpdateNarrative(request.Causes, request.Consequences, request.Notes);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
