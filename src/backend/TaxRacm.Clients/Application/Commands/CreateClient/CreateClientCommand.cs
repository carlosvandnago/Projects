using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Application.Commands.CreateClient;

/// <summary>Command to onboard a new KPMG client.</summary>
public record CreateClientCommand(
    string Name,
    string Industry,
    int FiscalYearEndMonth) : IRequest<Result<ClientDto>>;
