using TaxRacm.Clients.Domain.Enums;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Domain.Entities;

/// <summary>Aggregate root representing a KPMG client with their tax entity structure.</summary>
public class Client : AggregateRoot<ClientId>
{
    public string Name { get; private set; } = string.Empty;
    public string Industry { get; private set; } = string.Empty;
    public int FiscalYearEndMonth { get; private set; }
    public DateTime OnboardedDate { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<TaxEntity> _entities = new();
    public IReadOnlyCollection<TaxEntity> Entities => _entities.AsReadOnly();

    private Client() { } // EF Core

    /// <summary>Creates a new Client aggregate with no entities.</summary>
    public static Client Create(string name, string industry, int fiscalYearEndMonth)
    {
        var client = new Client
        {
            Id = ClientId.New(),
            Name = name,
            Industry = industry,
            FiscalYearEndMonth = fiscalYearEndMonth,
            OnboardedDate = DateTime.UtcNow,
            IsActive = true
        };
        return client;
    }

    /// <summary>Adds a tax entity to this client, enforcing no duplicate name+country combinations.</summary>
    public Result<TaxEntity> AddEntity(
        string name,
        CountryCode country,
        string jurisdiction,
        EntityType entityType,
        Region region)
    {
        if (_entities.Any(e => e.Name == name && e.Country == country))
            return Result<TaxEntity>.Failure("Entity with this name already exists in this country.");

        var entity = TaxEntity.Create(Id, name, country, jurisdiction, entityType, region);
        _entities.Add(entity);
        return Result<TaxEntity>.Success(entity);
    }

    /// <summary>Updates client-level properties.</summary>
    public void Update(string name, string industry, int fiscalYearEndMonth)
    {
        Name = name;
        Industry = industry;
        FiscalYearEndMonth = fiscalYearEndMonth;
    }

    /// <summary>Deactivates the client — does not delete data.</summary>
    public void Deactivate() => IsActive = false;
}
