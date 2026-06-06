using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.Clients.Domain.Enums;
using TaxRacm.Clients.Domain.Repositories;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Application.Commands.CreateEntity;

public class CreateEntityCommandHandler : IRequestHandler<CreateEntityCommand, Result<TaxEntityDto>>
{
    private readonly IClientRepository _clients;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEntityCommandHandler(IClientRepository clients, IUnitOfWork unitOfWork)
    {
        _clients = clients;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaxEntityDto>> Handle(CreateEntityCommand request, CancellationToken cancellationToken)
    {
        var client = await _clients.GetByIdWithEntitiesAsync(new ClientId(request.ClientId), cancellationToken);
        if (client is null)
            return Result<TaxEntityDto>.Failure("Client not found.");

        if (!Enum.TryParse<EntityType>(request.EntityType, out var entityType))
            return Result<TaxEntityDto>.Failure($"Invalid EntityType: {request.EntityType}");

        if (!Enum.TryParse<Region>(request.Region, out var region))
            return Result<TaxEntityDto>.Failure($"Invalid Region: {request.Region}");

        var country = new CountryCode(request.Country);
        var result = client.AddEntity(request.Name, country, request.Jurisdiction, entityType, region);
        if (result.IsFailure)
            return Result<TaxEntityDto>.Failure(result.Error!);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var entity = result.Value!;
        return Result<TaxEntityDto>.Success(new TaxEntityDto(
            entity.Id.Value,
            entity.ClientId.Value,
            entity.Name,
            entity.Country.Value,
            entity.Jurisdiction,
            entity.EntityType.ToString(),
            entity.Region.ToString(),
            entity.IsActive));
    }
}
