namespace TaxRacm.Clients.Application.DTOs;

public record TaxEntityDto(
    Guid Id,
    Guid ClientId,
    string Name,
    string Country,
    string Jurisdiction,
    string EntityType,
    string Region,
    bool IsActive);
