using MediatR;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.UnlinkEntityFromRisk;

public class UnlinkEntityFromRiskCommandHandler : IRequestHandler<UnlinkEntityFromRiskCommand, Result>
{
    private readonly IRacmRepository _racm;
    private readonly IUnitOfWork _unitOfWork;

    public UnlinkEntityFromRiskCommandHandler(IRacmRepository racm, IUnitOfWork unitOfWork)
    {
        _racm = racm;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UnlinkEntityFromRiskCommand request, CancellationToken cancellationToken)
    {
        var entry = await _racm.GetByIdAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        if (entry is null) return Result.Failure("RACM entry not found.");
        var result = entry.UnlinkEntity(new EntityId(request.EntityId));
        if (!result.IsSuccess) return result;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
