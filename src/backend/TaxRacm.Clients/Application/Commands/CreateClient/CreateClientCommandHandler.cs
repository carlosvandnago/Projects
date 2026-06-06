using MediatR;
using TaxRacm.Clients.Application.DTOs;
using TaxRacm.Clients.Domain.Entities;
using TaxRacm.Clients.Domain.Repositories;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Application.Commands.CreateClient;

/// <summary>Handles the CreateClientCommand by persisting a new Client aggregate.</summary>
public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<ClientDto>>
{
    private readonly IClientRepository _clients;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IClientRepository clients, IUnitOfWork unitOfWork)
    {
        _clients = clients;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = Client.Create(request.Name, request.Industry, request.FiscalYearEndMonth);
        _clients.Add(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = new ClientDto(
            client.Id.Value,
            client.Name,
            client.Industry,
            client.FiscalYearEndMonth,
            client.OnboardedDate,
            client.IsActive,
            Array.Empty<TaxEntityDto>());

        return Result<ClientDto>.Success(dto);
    }
}
