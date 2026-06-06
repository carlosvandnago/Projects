using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Risks.Domain.Repositories;

public interface IRacmRepository
{
    Task<RacmEntry?> GetByIdAsync(RacmEntryId id, CancellationToken ct = default);
    Task<IReadOnlyList<RacmEntry>> GetByClientAsync(ClientId clientId, CancellationToken ct = default);
    Task<RacmEntry?> GetByClientAndBankEntryAsync(ClientId clientId, RiskBankEntryId riskBankEntryId, CancellationToken ct = default);
    void Add(RacmEntry entry);
    void Update(RacmEntry entry);
}
