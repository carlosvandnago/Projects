using MediatR;
using TaxRacm.Controls.Application.DTOs;
using TaxRacm.Controls.Domain.Repositories;
using TaxRacm.Risks.Domain.ValueObjects;

namespace TaxRacm.Controls.Application.Queries.GetControlsByRisk;

public class GetControlsByRiskQueryHandler : IRequestHandler<GetControlsByRiskQuery, IReadOnlyList<ControlDto>>
{
    private readonly IControlRepository _controls;
    public GetControlsByRiskQueryHandler(IControlRepository controls) => _controls = controls;

    public async Task<IReadOnlyList<ControlDto>> Handle(GetControlsByRiskQuery request, CancellationToken cancellationToken)
    {
        var controls = await _controls.GetByRacmEntryAsync(new RacmEntryId(request.RacmEntryId), cancellationToken);
        return controls.Select(c => new ControlDto(
            c.Id.Value, c.RacmEntryId.Value, c.Name, c.Description,
            c.ControlType.ToString(), c.OwnerId.Value, c.Frequency.ToString(),
            c.LastTested, c.NextDue, c.EvidenceStatus.ToString(),
            c.RequiresReview, c.ReviewerId?.Value, c.ReviewStatus.ToString(),
            c.Effectiveness.ToString(),
            c.Evidence.Select(e => new EvidenceDto(e.Id, e.FileName, e.FileUrl, e.UploadedById.Value, e.Notes, e.UploadedAt)).ToList()
        )).ToList();
    }
}
