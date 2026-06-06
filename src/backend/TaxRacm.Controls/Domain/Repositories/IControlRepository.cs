using TaxRacm.Controls.Domain.Entities;
using TaxRacm.Controls.Domain.ValueObjects;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Controls.Domain.Repositories;

public interface IControlRepository
{
    Task<Control?> GetByIdAsync(ControlId id, CancellationToken ct = default);
    Task<IReadOnlyList<Control>> GetByRacmEntryAsync(RacmEntryId racmEntryId, CancellationToken ct = default);
    Task<IReadOnlyList<Control>> GetOverdueAsync(Guid clientId, CancellationToken ct = default);
    Task<IReadOnlyList<Control>> GetUpcomingDeadlinesAsync(Guid clientId, int daysAhead, CancellationToken ct = default);
    void Add(Control control);
    void Update(Control control);
}
