namespace CustomerService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    
    // Transaction management
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    
    // Check if transaction is active
    bool HasActiveTransaction { get; }
}