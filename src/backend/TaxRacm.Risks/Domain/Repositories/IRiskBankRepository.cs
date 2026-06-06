using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.Enums;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Risks.Domain.Repositories;

public interface IRiskBankRepository
{
    Task<RiskBankEntry?> GetByIdAsync(RiskBankEntryId id, CancellationToken ct = default);
    Task<IReadOnlyList<RiskBankEntry>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<RiskBankEntry>> GetFilteredAsync(TaxType? taxType, string? country, string? industry, CancellationToken ct = default);
    void Add(RiskBankEntry entry);
    void Update(RiskBankEntry entry);
}
