using Microsoft.EntityFrameworkCore;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.Enums;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.Risks.Infrastructure.Persistence;

namespace TaxRacm.Risks.Infrastructure.Repositories;

public class RiskBankRepository : IRiskBankRepository
{
    private readonly RisksDbContext _context;
    public RiskBankRepository(RisksDbContext context) => _context = context;

    public async Task<RiskBankEntry?> GetByIdAsync(RiskBankEntryId id, CancellationToken ct = default) =>
        await _context.RiskBankEntries.FindAsync(new object[] { id }, ct);

    public async Task<IReadOnlyList<RiskBankEntry>> GetAllAsync(CancellationToken ct = default) =>
        await _context.RiskBankEntries.Where(e => e.IsActive).OrderBy(e => e.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<RiskBankEntry>> GetFilteredAsync(TaxType? taxType, string? country, string? industry, CancellationToken ct = default)
    {
        var query = _context.RiskBankEntries.Where(e => e.IsActive).AsQueryable();
        if (taxType.HasValue) query = query.Where(e => e.TaxType == taxType.Value);
        return await query.OrderBy(e => e.Name).ToListAsync(ct);
    }

    public void Add(RiskBankEntry entry) => _context.RiskBankEntries.Add(entry);
    public void Update(RiskBankEntry entry) => _context.RiskBankEntries.Update(entry);
}
