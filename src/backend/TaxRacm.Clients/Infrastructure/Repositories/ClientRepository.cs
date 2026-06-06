using Microsoft.EntityFrameworkCore;
using TaxRacm.Clients.Domain.Entities;
using TaxRacm.Clients.Domain.Repositories;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Clients.Infrastructure.Persistence;

namespace TaxRacm.Clients.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ClientsDbContext _context;

    public ClientRepository(ClientsDbContext context) => _context = context;

    public async Task<Client?> GetByIdAsync(ClientId id, CancellationToken ct = default) =>
        await _context.Clients.FindAsync(new object[] { id }, ct);

    public async Task<Client?> GetByIdWithEntitiesAsync(ClientId id, CancellationToken ct = default) =>
        await _context.Clients
            .Include(c => c.Entities)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Clients
            .Include(c => c.Entities)
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public void Add(Client client) => _context.Clients.Add(client);
    public void Update(Client client) => _context.Clients.Update(client);
}
