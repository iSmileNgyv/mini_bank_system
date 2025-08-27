using System.Linq.Expressions;
using CustomerService.Domain.Entities.Common;
using CustomerService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Repositories;

public class ReadRepository<T> : BaseRepository<T>, IReadRepository<T> where T : BaseEntity
{
    public ReadRepository(CustomerDbContext context) : base(context)
    {
    }

    public IQueryable<T> GetAll(bool tracking = true)
    {
        return GetQueryable(tracking);
    }

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
    {
        return GetQueryable(tracking).Where(method);
    }

    public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true, CancellationToken cancellationToken = default)
    {
        return await GetQueryable(tracking).SingleOrDefaultAsync(method, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, bool tracking = true, CancellationToken cancellationToken = default)
    {
        return await GetQueryable(tracking).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> method, bool tracking = true, CancellationToken cancellationToken = default)
    {
        return await GetQueryable(tracking).FirstOrDefaultAsync(method, cancellationToken);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> method, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(method, cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> method, CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(method, cancellationToken);
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Max page size limit

        var query = GetQueryable(tracking);

        // Apply filter if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Get total count before paging
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply ordering if provided, otherwise order by Id
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        // Apply paging
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}