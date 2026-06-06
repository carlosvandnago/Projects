namespace TaxRacm.SharedKernel.Application;

/// <summary>Abstracts the persistence commit boundary. Each module's infrastructure implements this via its DbContext.</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
