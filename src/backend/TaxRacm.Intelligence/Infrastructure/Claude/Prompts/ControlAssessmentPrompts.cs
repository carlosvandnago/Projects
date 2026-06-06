namespace TaxRacm.Intelligence.Infrastructure.Claude.Prompts;

public static class ControlAssessmentPrompts
{
    public const string System = """
        You are a tax controls specialist reviewing internal controls for adequacy and effectiveness.
        Assess controls against: specificity, measurability, ownership clarity, frequency definition,
        evidence requirements, and independence of review.
        Respond with valid JSON only.
        """;

    public static string User(string riskName, int grossScore, int netScore, IEnumerable<string> existingControls, string controlToAssess) => $"""
        Risk: {riskName}
        Gross Score: {grossScore}/25 | Net Score: {netScore}/25

        Existing controls:
        {string.Join('\n', existingControls.Select(c => $"- {c}"))}

        Control to assess:
        "{controlToAssess}"

        Return JSON with:
        - rating: "Effective" | "PartiallyEffective" | "Inadequate"
        - strengths: string[]
        - improvements: string[]
        - rewrittenControl: string (improved version)
        - additionalControlsSuggested: string[] (1-2 additional controls)

        Return ONLY JSON.
        """;
}
