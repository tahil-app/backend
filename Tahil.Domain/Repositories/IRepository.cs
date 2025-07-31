namespace Tahil.Domain.Repositories;

public interface IRepository<T> where T : Base
{
    Task<T?> GetFirstAsync(Expression<Func<T, object>>[]? includes = null);
    Task<T?> GetLastAsync(Expression<Func<T, object>>[]? includes = null);

    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);
    Task<T?> GetReadOnlyAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);

    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);
    Task<List<T>> GetAllReadOnlyAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);
    Task<List<T>> GetDistinctListAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);
    Task<PagedList<T>> GetPagedAsync(QueryParams queryParams, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);

    Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);
    void Add(T entity);
    void Update(T entity);
    void HardDelete(T entity);
}