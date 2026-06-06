using System.Text.Json;
using MediatR;
using TaxRacm.Intelligence.Application.DTOs;
using TaxRacm.Intelligence.Domain.Entities;
using TaxRacm.Intelligence.Domain.Enums;
using TaxRacm.Intelligence.Domain.Interfaces;
using TaxRacm.Intelligence.Infrastructure.Claude;
using TaxRacm.Intelligence.Infrastructure.Claude.Prompts;
using TaxRacm.Intelligence.Infrastructure.Persistence;
using TaxRacm.Risks.Domain.ValueObjects;
using TaxRacm.SharedKernel.Application;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Intelligence.Application.Commands.AnalyseDocument;

public class AnalyseDocumentCommandHandler : IRequestHandler<AnalyseDocumentCommand, Result<DocumentAnalysisResultDto>>
{
    private readonly IClaudeClient _claude;
    private readonly IntelligenceDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ClaudeOptions _options;

    public AnalyseDocumentCommandHandler(IClaudeClient claude, IntelligenceDbContext context, IUnitOfWork unitOfWork, Microsoft.Extensions.Options.IOptions<ClaudeOptions> options)
    {
        _claude = claude;
        _context = context;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<DocumentAnalysisResultDto>> Handle(AnalyseDocumentCommand request, CancellationToken cancellationToken)
    {
        var aiRequest = AiRequest.Create(
            AiRequestType.DocumentAnalysis,
            new UserId(request.RequestedById),
            request.ClientId,
            JsonSerializer.Serialize(new { request.TaxType, request.EntityContext }),
            _options.Model);

        _context.AiRequests.Add(aiRequest);
        aiRequest.MarkProcessing();

        try
        {
            var response = await _claude.SendAsync(
                DocumentAnalysisPrompts.System,
                DocumentAnalysisPrompts.User(request.TaxType, request.EntityContext, request.DocumentText),
                _options.MaxTokens, cancellationToken);

            var risks = JsonSerializer.Deserialize<List<SuggestedRiskDto>>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<SuggestedRiskDto>();

            aiRequest.MarkCompleted(response.Content, response.TotalTokens);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<DocumentAnalysisResultDto>.Success(new DocumentAnalysisResultDto(aiRequest.Id, risks, response.TotalTokens));
        }
        catch (Exception ex)
        {
            aiRequest.MarkFailed(ex.Message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<DocumentAnalysisResultDto>.Failure($"AI analysis failed: {ex.Message}");
        }
    }
}
