using CustomerService.Domain.Entities;
using CustomerService.Domain.Enums;
using CustomerService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ReadRepository<Customer> _readRepository;
    private readonly WriteRepository<Customer> _writeRepository;
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _readRepository = new ReadRepository<Customer>(context);
        _writeRepository = new WriteRepository<Customer>(context);
    }

    #region IReadRepository Implementation
    public IQueryable<Customer> GetAll(bool tracking = true)
        => _readRepository.GetAll(tracking);

    public IQueryable<Customer> GetWhere(System.Linq.Expressions.Expression<Func<Customer, bool>> method, bool tracking = true)
        => _readRepository.GetWhere(method, tracking);

    public async Task<Customer?> GetSingleAsync(System.Linq.Expressions.Expression<Func<Customer, bool>> method, bool tracking = true, CancellationToken cancellationToken = default)
        => await _readRepository.GetSingleAsync(method, tracking, cancellationToken);

    public async Task<Customer?> GetByIdAsync(int id, bool tracking = true, CancellationToken cancellationToken = default)
        => await _readRepository.GetByIdAsync(id, tracking, cancellationToken);

    public async Task<Customer?> GetFirstAsync(System.Linq.Expressions.Expression<Func<Customer, bool>> method, bool tracking = true, CancellationToken cancellationToken = default)
        => await _readRepository.GetFirstAsync(method, tracking, cancellationToken);

    public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<Customer, bool>> method, CancellationToken cancellationToken = default)
        => await _readRepository.AnyAsync(method, cancellationToken);

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => await _readRepository.ExistsAsync(id, cancellationToken);

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _readRepository.CountAsync(cancellationToken);

    public async Task<int> CountAsync(System.Linq.Expressions.Expression<Func<Customer, bool>> method, CancellationToken cancellationToken = default)
        => await _readRepository.CountAsync(method, cancellationToken);

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, System.Linq.Expressions.Expression<Func<Customer, bool>>? filter = null, Func<IQueryable<Customer>, IOrderedQueryable<Customer>>? orderBy = null, bool tracking = true, CancellationToken cancellationToken = default)
        => await _readRepository.GetPagedAsync(page, pageSize, filter, orderBy, tracking, cancellationToken);
    #endregion

    #region IWriteRepository Implementation
    public async Task<Customer> AddAsync(Customer entity, CancellationToken cancellationToken = default)
        => await _writeRepository.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<Customer> entities, CancellationToken cancellationToken = default)
        => await _writeRepository.AddRangeAsync(entities, cancellationToken);

    public async Task UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
        => await _writeRepository.UpdateAsync(entity, cancellationToken);

    public async Task UpdateRangeAsync(IEnumerable<Customer> entities, CancellationToken cancellationToken = default)
        => await _writeRepository.UpdateRangeAsync(entities, cancellationToken);

    public async Task DeleteAsync(Customer entity, CancellationToken cancellationToken = default)
        => await _writeRepository.DeleteAsync(entity, cancellationToken);

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => await _writeRepository.DeleteAsync(id, cancellationToken);

    public async Task DeleteRangeAsync(IEnumerable<Customer> entities, CancellationToken cancellationToken = default)
        => await _writeRepository.DeleteRangeAsync(entities, cancellationToken);

    public async Task DeleteRangeAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        => await _writeRepository.DeleteRangeAsync(ids, cancellationToken);

    public async Task SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        => await _writeRepository.SoftDeleteAsync(id, cancellationToken);

    public async Task SoftDeleteAsync(Customer entity, CancellationToken cancellationToken = default)
        => await _writeRepository.SoftDeleteAsync(entity, cancellationToken);
    #endregion

    #region Customer-Specific Methods
    public async Task<Customer?> GetByCustomerNumberAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CustomerNumber == customerNumber, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Value == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.AccountStatus == AccountStatus.Active)
            .OrderBy(x => x.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetCustomersByBranchAsync(string branchCode, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.BranchCode == branchCode)
            .OrderBy(x => x.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetCustomersByRiskLevelAsync(RiskLevel riskLevel, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.RiskLevel == riskLevel)
            .OrderBy(x => x.FirstName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetCustomersByKycStatusAsync(KycStatus kycStatus, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(x => x.KycStatus == kycStatus)
            .OrderBy(x => x.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AnyAsync(x => x.CustomerNumber == customerNumber, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AnyAsync(x => x.Email.Value == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, int excludeCustomerId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AnyAsync(x => x.Email.Value == email.ToLowerInvariant() && x.Id != excludeCustomerId, cancellationToken);
    }

    public async Task<int> GetCustomersCountByBranchAsync(string branchCode, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .CountAsync(x => x.BranchCode == branchCode, cancellationToken);
    }

    public async Task<(IEnumerable<Customer> Customers, int TotalCount)> SearchCustomersAsync(
        string? searchTerm, 
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Customers.AsNoTracking();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLowerInvariant();
            query = query.Where(x => 
                x.FirstName.ToLower().Contains(searchTerm) ||
                x.LastName.ToLower().Contains(searchTerm) ||
                x.Email.Value.Contains(searchTerm) ||
                x.CustomerNumber.Contains(searchTerm));
        }

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply paging and ordering
        var customers = await query
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (customers, totalCount);
    }
    #endregion

    #region IDisposable
    public void Dispose()
    {
        _readRepository?.Dispose();
        _writeRepository?.Dispose();
        _context?.Dispose();
    }
    #endregion
}