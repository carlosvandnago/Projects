using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Controls.Application.Commands.SubmitEvidence;

public record SubmitEvidenceCommand(Guid ControlId, string FileName, string FileUrl, Guid UploadedById, string Notes) : IRequest<Result>;
