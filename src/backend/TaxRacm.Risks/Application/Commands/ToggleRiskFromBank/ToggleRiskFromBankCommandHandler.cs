using MediatR;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Commands.ToggleRiskFromBank;

public class ToggleRiskFromBankCommandHandler : IRequestHandler<ToggleRiskFromBankCommand, Result<Guid>>
{
    private readonly IRiskBankRepository _riskBank;
    private readonly IRacmRepository _racm;
    private readonly IUnitOfWork _unitOfWork;

    public ToggleRiskFromBankCommandHandler(IRiskBankRepository riskBank, IRacmRepository racm, IUnitOfWork unitOfWork)
    {
        _riskBank = riskBank;
        _racm = racm;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(ToggleRiskFromBankCommand request, CancellationToken cancellationToken)
    {
        var bankEntry = await _riskBank.GetByIdAsync(new RiskBankEntryId(request.RiskBankEntryId), cancellationToken);
        if (bankEntry is null)
            return Result<Guid>.Failure("Risk bank entry not found.");

        var existing = await _racm.GetByClientAndBankEntryAsync(
            new ClientId(request.ClientId),
            new RiskBankEntryId(request.RiskBankEntryId), cancellationToken);

        if (existing is not null)
            return Result<Guid>.Failure("Risk is already active in this client's RACM.");

        var racmEntry = RacmEntry.CreateFromBank(
            new ClientId(request.ClientId),
            bankEntry,
            new UserId(request.GlobalOwnerId),
            request.GrossLikelihood,
            request.GrossImpact);

        foreach (var entityId in request.LinkedEntityIds)
            racmEntry.LinkEntity(new EntityId(entityId));

        _racm.Add(racmEntry);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(racmEntry.Id.Value);
    }
}
