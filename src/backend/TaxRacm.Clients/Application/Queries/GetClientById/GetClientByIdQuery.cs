using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Application.Queries.GetClientById;

public record GetClientByIdQuery(Guid ClientId) : IRequest<Result<ClientDto>>;
