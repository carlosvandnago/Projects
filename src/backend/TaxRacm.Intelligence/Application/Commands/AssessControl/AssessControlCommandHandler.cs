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

namespace TaxRacm.Intelligence.Application.Commands.AssessControl;

public class AssessControlCommandHandler : IRequestHandler<AssessControlCommand, Result<ControlAssessmentDto>>
{
    private readonly IClaudeClient _claude;
    private readonly IntelligenceDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ClaudeOptions _options;

    public AssessControlCommandHandler(IClaudeClient claude, IntelligenceDbContext context, IUnitOfWork unitOfWork, Microsoft.Extensions.Options.IOptions<ClaudeOptions> options)
    {
        _claude = claude;
        _context = context;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<ControlAssessmentDto>> Handle(AssessControlCommand request, CancellationToken cancellationToken)
    {
        var aiRequest = AiRequest.Create(AiRequestType.ControlAssessment, new UserId(request.RequestedById), request.ClientId,
            JsonSerializer.Serialize(new { request.RiskName, request.ControlToAssess }), _options.Model);
        _context.AiRequests.Add(aiRequest);
        aiRequest.MarkProcessing();

        try
        {
            var response = await _claude.SendAsync(
                ControlAssessmentPrompts.System,
                ControlAssessmentPrompts.User(request.RiskName, request.GrossScore, request.NetScore, request.ExistingControls, request.ControlToAssess),
                _options.MaxTokens, cancellationToken);

            var result = JsonSerializer.Deserialize<AssessmentResult>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("Failed to parse control assessment response.");

            aiRequest.MarkCompleted(response.Content, response.TotalTokens);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ControlAssessmentDto>.Success(new ControlAssessmentDto(
                aiRequest.Id, result.Rating, result.Strengths, result.Improvements,
                result.RewrittenControl, result.AdditionalControlsSuggested, response.TotalTokens));
        }
        catch (Exception ex)
        {
            aiRequest.MarkFailed(ex.Message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ControlAssessmentDto>.Failure($"AI assessment failed: {ex.Message}");
        }
    }

    private record AssessmentResult(string Rating, List<string> Strengths, List<string> Improvements, string RewrittenControl, List<string> AdditionalControlsSuggested);
}
