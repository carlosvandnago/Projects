using MediatR;
using TaxRacm.Risks.Application.DTOs;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.Risks.Application.Queries.GetRiskById;

public record GetRiskByIdQuery(Guid RacmEntryId) : IRequest<Result<RacmEntryDto>>;
