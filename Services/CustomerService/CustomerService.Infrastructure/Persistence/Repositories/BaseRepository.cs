using CustomerService.Domain.Entities.Common;
using CustomerService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly CustomerDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(CustomerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    // Protected properties for derived classes
    protected CustomerDbContext Context => _context;
    protected DbSet<T> DbSet => _dbSet;

    // Common helper methods
    protected virtual IQueryable<T> GetQueryable(bool tracking = true)
    {
        return tracking ? _dbSet : _dbSet.AsNoTracking();
    }

    protected async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    // Disposal
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}