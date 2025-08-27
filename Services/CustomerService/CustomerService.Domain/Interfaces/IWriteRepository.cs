using CustomerService.Domain.Entities.Common;

namespace CustomerService.Domain.Interfaces;

public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    
    Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(T entity, CancellationToken cancellationToken = default);
}