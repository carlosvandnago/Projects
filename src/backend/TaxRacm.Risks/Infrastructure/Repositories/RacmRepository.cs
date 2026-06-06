using Microsoft.EntityFrameworkCore;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Entities;
using TaxRacm.Risks.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.Risks.Infrastructure.Persistence;

namespace TaxRacm.Risks.Infrastructure.Repositories;

public class RacmRepository : IRacmRepository
{
    private readonly RisksDbContext _context;
    public RacmRepository(RisksDbContext context) => _context = context;

    public async Task<RacmEntry?> GetByIdAsync(RacmEntryId id, CancellationToken ct = default) =>
        await _context.RacmEntries.Include(e => e.LinkedEntities)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<RacmEntry>> GetByClientAsync(ClientId clientId, CancellationToken ct = default) =>
        await _context.RacmEntries.Include(e => e.LinkedEntities)
            .Where(e => e.ClientId == clientId).OrderBy(e => e.Name).ToListAsync(ct);

    public async Task<RacmEntry?> GetByClientAndBankEntryAsync(ClientId clientId, RiskBankEntryId riskBankEntryId, CancellationToken ct = default) =>
        await _context.RacmEntries
            .FirstOrDefaultAsync(e => e.ClientId == clientId && e.RiskBankEntryId == riskBankEntryId, ct);

    public void Add(RacmEntry entry) => _context.RacmEntries.Add(entry);
    public void Update(RacmEntry entry) => _context.RacmEntries.Update(entry);
}
