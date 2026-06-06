namespace TaxRacm.Intelligence.Infrastructure.Claude.Prompts;

public static class DocumentAnalysisPrompts
{
    public const string System = """
        You are a senior tax risk specialist at a Big 4 professional services firm.
        You identify tax compliance risks from process documentation with precision.
        Always respond with valid JSON only — no preamble, no markdown code fences, no explanations outside the JSON.
        """;

    public static string User(string taxType, string entityContext, string documentText) => $"""
        Analyse the following {taxType} process documentation for {entityContext}.
        Identify all tax compliance risks and return a JSON array where each item has:
        - riskName: string (concise, specific)
        - description: string (2-3 sentences)
        - causes: string[] (2-4 root causes)
        - consequences: string[] (2-3 potential consequences)
        - suggestedPreventiveControls: string[] (2-3 specific, measurable controls)
        - suggestedMitigatingControls: string[] (1-2 controls)
        - defaultGrossLikelihood: number (1-5)
        - defaultGrossImpact: number (1-5)
        - taxType: string

        Return ONLY the JSON array. No other text.

        Document:
        {documentText}
        """;
}
