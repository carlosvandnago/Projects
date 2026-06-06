using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.Clients.Domain.Repositories;

namespace TaxRacm.Clients.Application.Queries.GetClients;

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IReadOnlyList<ClientDto>>
{
    private readonly IClientRepository _clients;

    public GetClientsQueryHandler(IClientRepository clients) => _clients = clients;

    public async Task<IReadOnlyList<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _clients.GetAllAsync(cancellationToken);
        return clients.Select(c => new ClientDto(
            c.Id.Value,
            c.Name,
            c.Industry,
            c.FiscalYearEndMonth,
            c.OnboardedDate,
            c.IsActive,
            c.Entities.Select(e => new TaxEntityDto(
                e.Id.Value, e.ClientId.Value, e.Name, e.Country.Value,
                e.Jurisdiction, e.EntityType.ToString(), e.Region.ToString(), e.IsActive)).ToList()))
        .ToList();
    }
}
