using CustomerService.Domain.Entities;
using CustomerService.Domain.Enums;

namespace CustomerService.Domain.Interfaces;

public interface ICustomerDomainService
{
    // Business rules validation
    Task<bool> IsEmailUniqueAsync(string email, int? excludeCustomerId = null, CancellationToken cancellationToken = default);
    Task<bool> IsCustomerNumberUniqueAsync(string customerNumber, CancellationToken cancellationToken = default);
    
    // Customer number generation
    Task<string> GenerateCustomerNumberAsync(string branchCode, CancellationToken cancellationToken = default);
    
    // Risk assessment
    Task<RiskLevel> CalculateRiskLevelAsync(Customer customer, CancellationToken cancellationToken = default);
    
    // Business validations
    Task<bool> CanCreateCustomerAsync(string email, string customerNumber, CancellationToken cancellationToken = default);
    Task<bool> CanUpdateCustomerAsync(int customerId, string email, CancellationToken cancellationToken = default);
    Task<bool> CanUploadProfilePhotoAsync(int customerId, long fileSize, CancellationToken cancellationToken = default);
    
    // KYC business logic
    Task<bool> IsKycUpdateValidAsync(Customer customer, KycStatus newStatus, CancellationToken cancellationToken = default);
}