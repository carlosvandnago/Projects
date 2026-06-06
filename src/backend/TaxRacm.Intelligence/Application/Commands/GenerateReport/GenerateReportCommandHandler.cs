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

namespace TaxRacm.Intelligence.Application.Commands.GenerateReport;

public class GenerateReportCommandHandler : IRequestHandler<GenerateReportCommand, Result<ReportDto>>
{
    private readonly IClaudeClient _claude;
    private readonly IntelligenceDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ClaudeOptions _options;

    public GenerateReportCommandHandler(IClaudeClient claude, IntelligenceDbContext context, IUnitOfWork unitOfWork, Microsoft.Extensions.Options.IOptions<ClaudeOptions> options)
    {
        _claude = claude;
        _context = context;
        _unitOfWork = unitOfWork;
        _options = options.Value;
    }

    public async Task<Result<ReportDto>> Handle(GenerateReportCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ReportType>(request.ReportType, out var reportType))
            return Result<ReportDto>.Failure($"Invalid ReportType: {request.ReportType}");

        var aiRequest = AiRequest.Create(AiRequestType.ReportGeneration, new UserId(request.RequestedById), request.ClientId,
            JsonSerializer.Serialize(new { request.ClientName, request.ReportType }), _options.Model);
        _context.AiRequests.Add(aiRequest);
        aiRequest.MarkProcessing();

        try
        {
            var genRequest = new GenerateReportRequest(request.ClientName, request.ReportType, request.EntityFilter, request.RacmEntries);
            var response = await _claude.SendAsync(
                ReportPrompts.GetSystemPrompt(reportType),
                ReportPrompts.UserPrompt(genRequest),
                4000, cancellationToken);

            aiRequest.MarkCompleted(response.Content, response.TotalTokens);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<ReportDto>.Success(new ReportDto(aiRequest.Id, request.ReportType, response.Content, response.TotalTokens, DateTime.UtcNow));
        }
        catch (Exception ex)
        {
            aiRequest.MarkFailed(ex.Message);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<ReportDto>.Failure($"Report generation failed: {ex.Message}");
        }
    }
}
