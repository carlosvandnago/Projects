using Microsoft.EntityFrameworkCore;
using TaxRacm.Controls.Domain.Entities;
using TaxRacm.Controls.Domain.Enums;
using TaxRacm.Controls.Domain.Repositories;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Controls.Infrastructure.Persistence;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Controls.Infrastructure.Repositories;

public class ControlRepository : IControlRepository
{
    private readonly ControlsDbContext _context;
    public ControlRepository(ControlsDbContext context) => _context = context;

    public async Task<Control?> GetByIdAsync(ControlId id, CancellationToken ct = default) =>
        await _context.Controls.Include(c => c.Evidence).FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Control>> GetByRacmEntryAsync(RacmEntryId racmEntryId, CancellationToken ct = default) =>
        await _context.Controls.Include(c => c.Evidence)
            .Where(c => c.RacmEntryId == racmEntryId).OrderBy(c => c.Name).ToListAsync(ct);

    public async Task<IReadOnlyList<Control>> GetOverdueAsync(Guid clientId, CancellationToken ct = default) =>
        await _context.Controls
            .Where(c => c.EvidenceStatus == EvidenceStatus.Overdue)
            .OrderBy(c => c.NextDue).ToListAsync(ct);

    public async Task<IReadOnlyList<Control>> GetUpcomingDeadlinesAsync(Guid clientId, int daysAhead, CancellationToken ct = default)
    {
        var cutoff = DateTime.UtcNow.AddDays(daysAhead);
        return await _context.Controls
            .Where(c => c.NextDue.HasValue && c.NextDue.Value <= cutoff && c.NextDue.Value >= DateTime.UtcNow)
            .OrderBy(c => c.NextDue).ToListAsync(ct);
    }

    public void Add(Control control) => _context.Controls.Add(control);
    public void Update(Control control) => _context.Controls.Update(control);
}
