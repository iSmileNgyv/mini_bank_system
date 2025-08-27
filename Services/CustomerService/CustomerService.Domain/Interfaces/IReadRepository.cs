using System.Linq.Expressions;
using CustomerService.Domain.Entities.Common;

namespace CustomerService.Domain.Interfaces;

public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
{
    IQueryable<T> GetAll(bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
    
    Task<T?> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, bool tracking = true, CancellationToken cancellationToken = default);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>> method, bool tracking = true, CancellationToken cancellationToken = default);
    
    Task<bool> AnyAsync(Expression<Func<T, bool>> method, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> method, CancellationToken cancellationToken = default);
    
    Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool tracking = true,
        CancellationToken cancellationToken = default);
}