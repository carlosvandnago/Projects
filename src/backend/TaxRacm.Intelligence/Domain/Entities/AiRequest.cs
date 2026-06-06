using TaxRacm.Intelligence.Domain.Enums;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Intelligence.Domain.Entities;

/// <summary>Full audit trail of every call made to the Claude API.</summary>
public class AiRequest : AggregateRoot<Guid>
{
    public AiRequestType RequestType { get; private set; }
    public UserId RequestedById { get; private set; } = null!;
    public Guid ClientId { get; private set; }
    public string InputData { get; private set; } = string.Empty;
    public string? OutputData { get; private set; }
    public AiRequestStatus Status { get; private set; }
    public string? ErrorMessage { get; private set; }
    public int? TokensUsed { get; private set; }
    public string ModelUsed { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private AiRequest() { }

    public static AiRequest Create(AiRequestType requestType, UserId requestedById, Guid clientId, string inputData, string modelUsed) => new()
    {
        Id = Guid.NewGuid(),
        RequestType = requestType,
        RequestedById = requestedById,
        ClientId = clientId,
        InputData = inputData,
        Status = AiRequestStatus.Pending,
        ModelUsed = modelUsed,
        CreatedAt = DateTime.UtcNow
    };

    public void MarkProcessing() => Status = AiRequestStatus.Processing;

    public void MarkCompleted(string outputData, int tokensUsed)
    {
        OutputData = outputData;
        TokensUsed = tokensUsed;
        Status = AiRequestStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void MarkFailed(string errorMessage)
    {
        ErrorMessage = errorMessage;
        Status = AiRequestStatus.Failed;
        CompletedAt = DateTime.UtcNow;
    }
}
