using TaxRacm.Clients.Domain.Entities;
using TaxRacm.Clients.Domain.ValueObjects;

namespace TaxRacm.Clients.Domain.Repositories;

/// <summary>Repository contract for the Client aggregate. Implementations live in Infrastructure.</summary>
public interface IClientRepository
{
    Task<Client?> GetByIdAsync(ClientId id, CancellationToken ct = default);
    Task<Client?> GetByIdWithEntitiesAsync(ClientId id, CancellationToken ct = default);
    Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken ct = default);
    void Add(Client client);
    void Update(Client client);
}
