using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.Clients.Domain.Repositories;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Application.Queries.GetClientById;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<ClientDto>>
{
    private readonly IClientRepository _clients;

    public GetClientByIdQueryHandler(IClientRepository clients) => _clients = clients;

    public async Task<Result<ClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clients.GetByIdWithEntitiesAsync(new ClientId(request.ClientId), cancellationToken);
        if (client is null)
            return Result<ClientDto>.Failure("Client not found.");

        return Result<ClientDto>.Success(new ClientDto(
            client.Id.Value,
            client.Name,
            client.Industry,
            client.FiscalYearEndMonth,
            client.OnboardedDate,
            client.IsActive,
            client.Entities.Select(e => new TaxEntityDto(
                e.Id.Value, e.ClientId.Value, e.Name, e.Country.Value,
                e.Jurisdiction, e.EntityType.ToString(), e.Region.ToString(), e.IsActive)).ToList()));
    }
}
