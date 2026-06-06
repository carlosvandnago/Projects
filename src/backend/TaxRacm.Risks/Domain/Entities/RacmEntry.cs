using TaxRacm.Clients.Domain.ValueObjects;
using TaxRacm.Risks.Domain.Enums;
using TaxRacm.Risks.Domain.Events;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Domain.Entities;

/// <summary>Aggregate root — the core business object representing an active risk in a client's RACM.</summary>
public class RacmEntry : AggregateRoot<RacmEntryId>
{
    public ClientId ClientId { get; private set; } = null!;
    public RiskBankEntryId? RiskBankEntryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TaxType TaxType { get; private set; }
    public RiskScope Scope { get; private set; }
    public UserId GlobalOwnerId { get; private set; } = null!;
    public RiskScore GrossScore { get; private set; } = null!;
    public RiskScore NetScore { get; private set; } = null!;
    public List<string> Causes { get; private set; } = new();
    public List<string> Consequences { get; private set; } = new();
    public RacmStatus Status { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public DateTime LastReviewed { get; private set; }

    private readonly List<RacmEntityLink> _linkedEntities = new();
    public IReadOnlyCollection<RacmEntityLink> LinkedEntities => _linkedEntities.AsReadOnly();

    public RiskRating GrossRiskRating => RiskRating.FromScore(GrossScore.Score);
    public RiskRating NetRiskRating => RiskRating.FromScore(NetScore.Score);
    public int ControlEffectivenessDelta => GrossScore.Score - NetScore.Score;

    private RacmEntry() { }

    /// <summary>Creates a RACM entry from a risk bank entry, copying causes and consequences.</summary>
    public static RacmEntry CreateFromBank(
        ClientId clientId,
        RiskBankEntry bankEntry,
        UserId globalOwnerId,
        int grossLikelihood,
        int grossImpact)
    {
        var entry = new RacmEntry
        {
            Id = RacmEntryId.New(),
            ClientId = clientId,
            RiskBankEntryId = bankEntry.Id,
            Name = bankEntry.Name,
            Description = bankEntry.Description,
            TaxType = bankEntry.TaxType,
            Scope = RiskScope.Global,
            GlobalOwnerId = globalOwnerId,
            GrossScore = RiskScore.Create(grossLikelihood, grossImpact),
            NetScore = RiskScore.Create(grossLikelihood, grossImpact),
            Causes = new List<string>(bankEntry.Causes),
            Consequences = new List<string>(bankEntry.Consequences),
            Status = RacmStatus.Active,
            LastReviewed = DateTime.UtcNow,
        };
        entry.RaiseDomainEvent(new RiskActivatedEvent(entry.Id, clientId.Value));
        return entry;
    }

    /// <summary>Creates a custom RACM entry not linked to the risk bank.</summary>
    public static RacmEntry CreateCustom(
        ClientId clientId,
        string name,
        string description,
        TaxType taxType,
        RiskScope scope,
        UserId globalOwnerId,
        int grossLikelihood,
        int grossImpact,
        List<string> causes,
        List<string> consequences) => new()
    {
        Id = RacmEntryId.New(),
        ClientId = clientId,
        Name = name,
        Description = description,
        TaxType = taxType,
        Scope = scope,
        GlobalOwnerId = globalOwnerId,
        GrossScore = RiskScore.Create(grossLikelihood, grossImpact),
        NetScore = RiskScore.Create(grossLikelihood, grossImpact),
        Causes = causes,
        Consequences = consequences,
        Status = RacmStatus.Active,
        LastReviewed = DateTime.UtcNow,
    };

    /// <summary>Updates the net risk score after controls are applied.</summary>
    public Result UpdateNetScore(int likelihood, int impact)
    {
        if (likelihood < 1 || likelihood > 5) return Result.Failure("Likelihood must be 1-5.");
        if (impact < 1 || impact > 5) return Result.Failure("Impact must be 1-5.");
        var oldScore = NetScore;
        NetScore = RiskScore.Create(likelihood, impact);
        RaiseDomainEvent(new RiskScoreChangedEvent(Id, oldScore, NetScore));
        LastReviewed = DateTime.UtcNow;
        return Result.Success();
    }

    /// <summary>Links a tax entity to this risk. Idempotent.</summary>
    public void LinkEntity(EntityId entityId, UserId? localOwnerId = null)
    {
        if (_linkedEntities.Any(l => l.EntityId == entityId)) return;
        _linkedEntities.Add(new RacmEntityLink(Id, entityId, localOwnerId));
    }

    /// <summary>Removes a linked entity.</summary>
    public Result UnlinkEntity(EntityId entityId)
    {
        var link = _linkedEntities.FirstOrDefault(l => l.EntityId == entityId);
        if (link is null) return Result.Failure("Entity not linked to this risk.");
        _linkedEntities.Remove(link);
        return Result.Success();
    }

    public void UpdateNarrative(List<string> causes, List<string> consequences, string notes)
    {
        Causes = causes;
        Consequences = consequences;
        Notes = notes;
        LastReviewed = DateTime.UtcNow;
    }

    public void Close() => Status = RacmStatus.Closed;
    public void SetUnderReview() => Status = RacmStatus.InReview;
    public void Activate() => Status = RacmStatus.Active;
}
