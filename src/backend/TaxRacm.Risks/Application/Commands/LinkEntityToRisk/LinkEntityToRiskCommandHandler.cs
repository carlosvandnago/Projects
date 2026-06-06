using MediatR;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.LinkEntityToRisk;

public class LinkEntityToRiskCommandHandler : IRequestHandler<LinkEntityToRiskCommand, Result>
{
    private readonly IRacmRepository _racm;
    private readonly IUnitOfWork _unitOfWork;

    public LinkEntityToRiskCommandHandler(IRacmRepository racm, IUnitOfWork unitOfWork)
    {
        _racm = racm;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LinkEntityToRiskCommand request, CancellationToken cancellationToken)
    {
        var entry = await _racm.GetByIdAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        if (entry is null) return Result.Failure("RACM entry not found.");
        entry.LinkEntity(
            new EntityId(request.EntityId),
            request.LocalOwnerId.HasValue ? new UserId(request.LocalOwnerId.Value) : null);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
