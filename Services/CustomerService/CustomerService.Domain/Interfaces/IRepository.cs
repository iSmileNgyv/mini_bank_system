using CustomerService.Domain.Entities.Common;

namespace CustomerService.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    // Infrastructure-specific implementations will be added later
}