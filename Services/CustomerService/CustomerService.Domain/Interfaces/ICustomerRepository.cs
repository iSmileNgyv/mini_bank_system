using CustomerService.Domain.Entities;
using CustomerService.Domain.Enums;

namespace CustomerService.Domain.Interfaces;

public interface ICustomerRepository : IReadRepository<Customer>, IWriteRepository<Customer>
{
    Task<Customer?> GetByCustomerNumberAsync(string customerNumber, CancellationToken cancellationToken = default);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Customer>> GetCustomersByBranchAsync(string branchCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetCustomersByRiskLevelAsync(RiskLevel riskLevel, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetCustomersByKycStatusAsync(KycStatus kycStatus, CancellationToken cancellationToken = default);
    
    Task<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, int excludeCustomerId, CancellationToken cancellationToken = default);
    
    Task<int> GetCustomersCountByBranchAsync(string branchCode, CancellationToken cancellationToken = default);
    
    Task<(IEnumerable<Customer> Customers, int TotalCount)> SearchCustomersAsync(
        string? searchTerm, 
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default);
}