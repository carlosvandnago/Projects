using MediatR;
using TaxRacm.Controls.Application.DTOs;
using TaxRacm.Controls.Domain.Repositories;

namespace TaxRacm.Controls.Application.Queries.GetOverdueControls;

public class GetOverdueControlsQueryHandler : IRequestHandler<GetOverdueControlsQuery, IReadOnlyList<ControlDto>>
{
    private readonly IControlRepository _controls;
    public GetOverdueControlsQueryHandler(IControlRepository controls) => _controls = controls;

    public async Task<IReadOnlyList<ControlDto>> Handle(GetOverdueControlsQuery request, CancellationToken cancellationToken)
    {
        var controls = await _controls.GetOverdueAsync(request.ClientId, cancellationToken);
        return controls.Select(c => new ControlDto(
            c.Id.Value, c.RacmEntryId.Value, c.Name, c.Description,
            c.ControlType.ToString(), c.OwnerId.Value, c.Frequency.ToString(),
            c.LastTested, c.NextDue, c.EvidenceStatus.ToString(),
            c.RequiresReview, c.ReviewerId?.Value, c.ReviewStatus.ToString(),
            c.Effectiveness.ToString(), new List<EvidenceDto>())).ToList();
    }
}
