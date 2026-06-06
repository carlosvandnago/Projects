namespace TaxRacm.Intelligence.Domain.Interfaces;

public record ClaudeResponse(string Content, int TotalTokens);

public interface IClaudeClient
{
    Task<ClaudeResponse> SendAsync(string systemPrompt, string userPrompt, int maxTokens = 2000, CancellationToken ct = default);
}
