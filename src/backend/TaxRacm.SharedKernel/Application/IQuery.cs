using MediatR;

namespace TaxRacm.SharedKernel.Application;

/// <summary>Marker interface for MediatR queries.</summary>
public interface IQuery<TResponse> : IRequest<TResponse> { }
