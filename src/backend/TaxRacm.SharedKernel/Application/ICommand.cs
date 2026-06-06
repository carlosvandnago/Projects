using MediatR;
using TaxRacm.SharedKernel.Domain;

namespace TaxRacm.SharedKernel.Application;

/// <summary>Marker interface for MediatR commands returning a typed result.</summary>
public interface ICommand<TResponse> : IRequest<TResponse> { }

/// <summary>Marker interface for MediatR commands returning Result.</summary>
public interface ICommand : IRequest<Result> { }
