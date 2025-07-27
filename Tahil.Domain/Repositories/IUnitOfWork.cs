namespace Tahil.Domain.Repositories;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync();
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}