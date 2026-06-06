using TaxRacm.Intelligence.Application.DTOs;
using TaxRacm.Intelligence.Domain.Enums;

namespace TaxRacm.Intelligence.Infrastructure.Claude.Prompts;

public static class ReportPrompts
{
    public static string GetSystemPrompt(ReportType reportType) => reportType switch
    {
        ReportType.LocalEntity => """
            You are a tax compliance advisor preparing a clear, action-oriented briefing for a local finance team.
            Write in plain, direct language. Focus on what they need to do and by when. Use markdown formatting.
            """,
        ReportType.GlobalHeadOfTax => """
            You are a tax technology director preparing a portfolio-level risk briefing for a Group Head of Tax.
            Write with analytical rigour. Include specific data points, entity comparisons, and trend observations.
            Recommend specific actions. Use markdown formatting with clear headings.
            """,
        ReportType.BoardAuditCommittee => """
            You are preparing an audit committee briefing on the group's tax risk environment.
            Write in plain English for non-tax specialists. Avoid jargon.
            Frame risks in business terms — financial exposure, reputational risk, regulatory relationship.
            Keep it concise and strategic. Use markdown formatting.
            """,
        ReportType.KpmgAdvisory => """
            You are a KPMG tax advisory director preparing an internal advisory intelligence briefing.
            Identify commercial opportunities to deepen the client relationship.
            Frame observations as advisory opportunities. Be specific about which service lines and which
            partner conversations should follow. This is internal only — the client will never see this report.
            Use markdown formatting.
            """,
        _ => throw new ArgumentOutOfRangeException(nameof(reportType))
    };

    public static string UserPrompt(GenerateReportRequest request) => $"""
        Prepare a report for {request.ClientName} based on the following RACM data:

        {FormatRacmData(request.RacmEntries)}

        {(request.EntityFilter is not null ? $"Focus on entity: {request.EntityFilter}" : "Cover all entities.")}

        Generate a professional, well-structured report with clear findings and recommended actions.
        """;

    private static string FormatRacmData(IEnumerable<RacmSummaryDto> entries) =>
        string.Join("\n---\n", entries.Select(r => $"""
            Risk: {r.Name}
            Tax Type: {r.TaxType}
            Gross Score: {r.GrossScore}/25 | Net Score: {r.NetScore}/25 | Delta: {r.GrossScore - r.NetScore}
            Rating: {r.NetRating}
            Controls: {r.TotalControls} total, {r.EvidencedControls} evidenced
            Outstanding: {r.OutstandingActions} controls need attention
            Notes: {r.Notes ?? "None"}
            """));
}
