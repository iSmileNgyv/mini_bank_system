using CustomerService.Domain.Entities;
using CustomerService.Domain.Enums;
using CustomerService.Domain.Interfaces;

namespace CustomerService.Infrastructure.Persistence.Services;

public class CustomerDomainService : ICustomerDomainService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerDomainService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? excludeCustomerId = null, CancellationToken cancellationToken = default)
    {
        if (excludeCustomerId.HasValue)
        {
            return !await _customerRepository.EmailExistsAsync(email, excludeCustomerId.Value, cancellationToken);
        }
        
        return !await _customerRepository.EmailExistsAsync(email, cancellationToken);
    }

    public async Task<bool> IsCustomerNumberUniqueAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return !await _customerRepository.CustomerNumberExistsAsync(customerNumber, cancellationToken);
    }

    public async Task<string> GenerateCustomerNumberAsync(string branchCode, CancellationToken cancellationToken = default)
    {
        const int maxAttempts = 10;
        var attempts = 0;
        
        while (attempts < maxAttempts)
        {
            var customerNumber = GenerateCustomerNumberFormat(branchCode);
            var isUnique = await IsCustomerNumberUniqueAsync(customerNumber, cancellationToken);
            
            if (isUnique)
            {
                return customerNumber;
            }
            
            attempts++;
            await Task.Delay(10, cancellationToken); // Small delay before retry
        }
        
        throw new InvalidOperationException($"Unable to generate unique customer number after {maxAttempts} attempts");
    }

    public async Task<RiskLevel> CalculateRiskLevelAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var riskScore = 0;
        
        // Age-based risk (younger = higher risk)
        var age = customer.GetAge();
        if (age < 25) riskScore += 2;
        else if (age < 35) riskScore += 1;
        
        // Customer type risk
        switch (customer.CustomerType)
        {
            case CustomerType.Individual:
                riskScore += 1;
                break;
            case CustomerType.Corporate:
                riskScore += 2;
                break;
            case CustomerType.Government:
                riskScore += 0; // Lowest risk
                break;
        }
        
        // Country-based risk (simple example)
        var highRiskCountries = new[] { "North Korea", "Iran", "Syria" };
        if (highRiskCountries.Contains(customer.Address.Country, StringComparer.OrdinalIgnoreCase))
        {
            riskScore += 3;
        }
        
        // Check existing customers count (suspicious if many from same address)
        var customersFromSameAddress = await _customerRepository.CountAsync(
            x => x.Address.Street == customer.Address.Street && 
                 x.Address.City == customer.Address.City, 
            cancellationToken);
                 
        if (customersFromSameAddress > 3)
        {
            riskScore += 2;
        }

        // Determine risk level
        return riskScore switch
        {
            <= 2 => RiskLevel.Low,
            <= 4 => RiskLevel.Medium,
            <= 6 => RiskLevel.High,
            _ => RiskLevel.Critical
        };
    }

    public async Task<bool> CanCreateCustomerAsync(string email, string customerNumber, CancellationToken cancellationToken = default)
    {
        // Email must be unique
        var emailExists = await _customerRepository.EmailExistsAsync(email, cancellationToken);
        if (emailExists)
            return false;
            
        // Customer number must be unique (if provided)
        if (!string.IsNullOrWhiteSpace(customerNumber))
        {
            var numberExists = await _customerRepository.CustomerNumberExistsAsync(customerNumber, cancellationToken);
            if (numberExists)
                return false;
        }
        
        return true;
    }

    public async Task<bool> CanUpdateCustomerAsync(int customerId, string email, CancellationToken cancellationToken = default)
    {
        // Email must be unique (excluding current customer)
        return await IsEmailUniqueAsync(email, customerId, cancellationToken);
    }

    public async Task<bool> CanUploadProfilePhotoAsync(int customerId, long fileSize, CancellationToken cancellationToken = default)
    {
        // Business rules for photo upload
        const long maxFileSize = 5 * 1024 * 1024; // 5MB
        
        if (fileSize > maxFileSize)
            return false;
            
        // Check if customer exists and is active
        var customer = await _customerRepository.GetByIdAsync(customerId, false, cancellationToken);
        if (customer == null || customer.AccountStatus != AccountStatus.Active)
            return false;
            
        return true;
    }

    public async Task<bool> IsKycUpdateValidAsync(Customer customer, KycStatus newStatus, CancellationToken cancellationToken = default)
    {
        // Business rules for KYC status changes
        return customer.KycStatus switch
        {
            KycStatus.Pending => newStatus is KycStatus.InProgress or KycStatus.Rejected,
            KycStatus.InProgress => newStatus is KycStatus.Completed or KycStatus.Rejected,
            KycStatus.Completed => newStatus == KycStatus.Expired, // Can only expire
            KycStatus.Rejected => newStatus == KycStatus.Pending, // Can restart process
            KycStatus.Expired => newStatus == KycStatus.Pending, // Can restart process
            _ => false
        };
    }

    #region Private Helper Methods
    private static string GenerateCustomerNumberFormat(string branchCode)
    {
        var branch = (branchCode ?? "DEF").ToUpperInvariant();
        if (branch.Length > 3) branch = branch[..3]; // Max 3 characters
        
        var year = DateTime.UtcNow.ToString("yy");
        var sequence = DateTime.UtcNow.ToString("MMddHHmm");
        var random = new Random().Next(100, 999);
        
        return $"{branch}{year}{sequence}{random}";
    }
    #endregion
}