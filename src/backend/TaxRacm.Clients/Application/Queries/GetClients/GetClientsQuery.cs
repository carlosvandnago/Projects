using MediatR;
using TaxRacm.Clients.Application.DTOs;

namespace TaxRacm.Clients.Application.Queries.GetClients;

public record GetClientsQuery() : IRequest<IReadOnlyList<ClientDto>>;
