using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Application.Commands.CreateEntity;

/// <summary>Adds a tax entity to an existing client.</summary>
public record CreateEntityCommand(
    Guid ClientId,
    string Name,
    string Country,
    string Jurisdiction,
    string EntityType,
    string Region) : IRequest<Result<TaxEntityDto>>;
