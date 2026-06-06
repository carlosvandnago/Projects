namespace TaxRacm.Intelligence.Application.DTOs;

public record ReportDto(Guid AiRequestId, string ReportType, string MarkdownContent, int TokensUsed, DateTime GeneratedAt);

public record GenerateReportRequest(string ClientName, string ReportType, string? EntityFilter, List<RacmSummaryDto> RacmEntries);

public record RacmSummaryDto(string Name, string TaxType, int GrossScore, int NetScore, string NetRating, int TotalControls, int EvidencedControls, int OutstandingActions, string? Notes);
