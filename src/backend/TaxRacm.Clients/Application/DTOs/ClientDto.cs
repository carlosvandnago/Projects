namespace TaxRacm.Clients.Application.DTOs;

public record ClientDto(
    Guid Id,
    string Name,
    string Industry,
    int FiscalYearEndMonth,
    DateTime OnboardedDate,
    bool IsActive,
    IReadOnlyList<TaxEntityDto> Entities);
