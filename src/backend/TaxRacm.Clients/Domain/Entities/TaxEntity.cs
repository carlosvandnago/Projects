using TaxRacm.Clients.Domain.Enums;
using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Clients.Domain.Entities;

/// <summary>Represents a legal tax entity within a client group (e.g. subsidiary, branch).</summary>
public class TaxEntity : Entity<EntityId>
{
    public ClientId ClientId { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public CountryCode Country { get; private set; } = null!;
    public string Jurisdiction { get; private set; } = string.Empty;
    public EntityType EntityType { get; private set; }
    public Region Region { get; private set; }
    public bool IsActive { get; private set; } = true;

    private TaxEntity() { } // EF Core

    /// <summary>Creates a new TaxEntity. Internal — only called from Client aggregate.</summary>
    internal static TaxEntity Create(
        ClientId clientId,
        string name,
        CountryCode country,
        string jurisdiction,
        EntityType entityType,
        Region region) => new()
    {
        Id = EntityId.New(),
        ClientId = clientId,
        Name = name,
        Country = country,
        Jurisdiction = jurisdiction,
        EntityType = entityType,
        Region = region,
        IsActive = true
    };

    /// <summary>Deactivates the entity rather than deleting it.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Updates mutable entity properties.</summary>
    public void Update(string name, string jurisdiction, EntityType entityType, Region region)
    {
        Name = name;
        Jurisdiction = jurisdiction;
        EntityType = entityType;
        Region = region;
    }
}
