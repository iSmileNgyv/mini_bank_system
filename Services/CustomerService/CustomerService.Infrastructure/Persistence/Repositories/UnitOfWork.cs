using CustomerService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerService.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CustomerDbContext _context;
    private IDbContextTransaction? _transaction;
    
    // Lazy initialization for repositories
    private ICustomerRepository? _customers;

    public UnitOfWork(CustomerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Repository properties with lazy initialization
    public ICustomerRepository Customers => 
        _customers ??= new CustomerRepository(_context);

    // Transaction management
    public bool HasActiveTransaction => _transaction != null;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            // Log the exception if needed
            throw;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            return; // No transaction to rollback
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Dispose pattern
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose transaction if active
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                }

                // Dispose context
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}