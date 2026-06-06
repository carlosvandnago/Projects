using MediatR;
using TaxRacm.Intelligence.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Intelligence.Application.Commands.AnalyseDocument;

public record AnalyseDocumentCommand(Guid ClientId, Guid RequestedById, string TaxType, string EntityContext, string DocumentText) : IRequest<Result<DocumentAnalysisResultDto>>;
