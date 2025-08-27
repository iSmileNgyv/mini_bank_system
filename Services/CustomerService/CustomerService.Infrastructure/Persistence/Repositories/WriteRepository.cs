using CustomerService.Domain.Entities.Common;
using CustomerService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Repositories;

public class WriteRepository<T> : BaseRepository<T>, IWriteRepository<T> where T : BaseEntity
{
    public WriteRepository(CustomerDbContext context) : base(context)
    {
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var result = await _dbSet.AddAsync(entity, cancellationToken);
        return result.Entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        var entitiesList = entities.ToList();
        if (!entitiesList.Any())
            return;

        await _dbSet.AddRangeAsync(entitiesList, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        var entitiesList = entities.ToList();
        if (!entitiesList.Any())
            return Task.CompletedTask;

        _dbSet.UpdateRange(entitiesList);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        var entitiesList = entities.ToList();
        if (!entitiesList.Any())
            return Task.CompletedTask;

        _dbSet.RemoveRange(entitiesList);
        return Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        if (ids == null)
            throw new ArgumentNullException(nameof(ids));

        var idsList = ids.ToList();
        if (!idsList.Any())
            return;

        var entities = await _dbSet
            .Where(x => idsList.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (entities.Any())
        {
            _dbSet.RemoveRange(entities);
        }
    }

    public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity != null)
        {
            // Implement soft delete logic if BaseEntity supports it
            // For now, just mark as updated
            entity.SetUpdatedDate();
            _dbSet.Update(entity);
        }
    }

    public Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        // Implement soft delete logic if BaseEntity supports it  
        // For now, just mark as updated
        entity.SetUpdatedDate();
        _dbSet.Update(entity);
        
        return Task.CompletedTask;
    }
}