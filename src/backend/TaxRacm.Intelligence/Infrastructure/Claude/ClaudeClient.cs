using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaxRacm.Intelligence.Domain.Interfaces;

namespace TaxRacm.Intelligence.Infrastructure.Claude;

public class ClaudeClient : IClaudeClient
{
    private readonly HttpClient _httpClient;
    private readonly ClaudeOptions _options;
    private readonly ILogger<ClaudeClient> _logger;

    public ClaudeClient(HttpClient httpClient, IOptions<ClaudeOptions> options, ILogger<ClaudeClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("https://api.anthropic.com");
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    }

    public async Task<ClaudeResponse> SendAsync(string systemPrompt, string userPrompt, int maxTokens = 2000, CancellationToken ct = default)
    {
        var requestBody = new
        {
            model = _options.Model,
            max_tokens = maxTokens,
            system = systemPrompt,
            messages = new[] { new { role = "user", content = userPrompt } }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Sending request to Claude API. Model: {Model}", _options.Model);
        var response = await _httpClient.PostAsync("/v1/messages", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var claudeResponse = JsonSerializer.Deserialize<ClaudeApiResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Failed to deserialise Claude response.");

        return new ClaudeResponse(
            claudeResponse.Content.First().Text,
            claudeResponse.Usage.InputTokens + claudeResponse.Usage.OutputTokens);
    }

    private record ClaudeApiResponse(
        [property: JsonPropertyName("content")] List<ContentBlock> Content,
        [property: JsonPropertyName("usage")] UsageInfo Usage);

    private record ContentBlock([property: JsonPropertyName("text")] string Text);

    private record UsageInfo(
        [property: JsonPropertyName("input_tokens")] int InputTokens,
        [property: JsonPropertyName("output_tokens")] int OutputTokens);
}
