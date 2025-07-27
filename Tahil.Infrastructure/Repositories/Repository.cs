using Tahil.Infrastructure.Extensions;

namespace Tahil.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : Base
{
    public readonly DbSet<T> _dbSet;
    virtual protected IQueryable<T> _query { get => _dbSet; }

    public Repository(DbSet<T> dbSet)
    {
        _dbSet = dbSet;
    }

    public Task<T?> GetFirstAsync(Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        return query.FirstOrDefaultAsync();
    }
    public Task<T?> GetLastAsync(Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        return query.LastOrDefaultAsync();
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        return query.FirstOrDefaultAsync(predicate);
    }
    public Task<T?> GetReadOnlyAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        return query.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        query = predicate != null ? query.Where(predicate) : query;
        return query.ToListAsync();
    }
    public Task<List<T>> GetAllReadOnlyAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        query = predicate != null ? query.Where(predicate) : query;
        return query.AsNoTracking().ToListAsync();
    }
    public Task<List<T>> GetDistinctListAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        query = predicate != null ? query.Where(predicate) : query;
        return query.Distinct().ToListAsync();
    }
    public async Task<PagedList<T>> GetPagedAsync(QueryParams queryParams, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);

        query = predicate != null ? query.Where(predicate) : query;

        query = queryParams.Filters != null ? query.ApplyFilters(queryParams.Filters) : query;

        query = queryParams.Sort != null ? query.ApplySorting(queryParams.Sort) : query.OrderByDescending(r => r.Id);

        var totalCount = await query.CountAsync();
        var items = await query.Skip((queryParams.Page - 1) * queryParams.PageSize).Take(queryParams.PageSize).ToListAsync();

        return new PagedList<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    public void Add(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(T));
        }

        _dbSet.Add(entity);
    }
    public void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(T));
        }

        _dbSet.Update(entity);
    }
    public void HardDelete(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(T));
        }

        _dbSet.Remove(entity);
    }

    protected IQueryable<T> Include(Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = _query;
        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return query;
    }
}
