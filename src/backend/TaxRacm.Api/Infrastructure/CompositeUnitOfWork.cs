using TaxRacm.Clients.Infrastructure.Persistence;
using TaxRacm.Controls.Infrastructure.Persistence;
using TaxRacm.Intelligence.Infrastructure.Persistence;
using TaxRacm.Risks.Infrastructure.Persistence;
using TaxRacm.SharedKernel.Application;

namespace TaxRacm.Api.Infrastructure;

public class CompositeUnitOfWork : IUnitOfWork
{
    private readonly ClientsDbContext _clients;
    private readonly RisksDbContext _risks;
    private readonly ControlsDbContext _controls;
    private readonly IntelligenceDbContext _intelligence;

    public CompositeUnitOfWork(
        ClientsDbContext clients,
        RisksDbContext risks,
        ControlsDbContext controls,
        IntelligenceDbContext intelligence)
    {
        _clients = clients;
        _risks = risks;
        _controls = controls;
        _intelligence = intelligence;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var total = 0;
        total += await _clients.SaveChangesAsync(cancellationToken);
        total += await _risks.SaveChangesAsync(cancellationToken);
        total += await _controls.SaveChangesAsync(cancellationToken);
        total += await _intelligence.SaveChangesAsync(cancellationToken);
        return total;
    }
}
