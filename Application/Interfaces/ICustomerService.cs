using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ErrorOr<Customer>> CreateCustomerAsync(Customer customer);
        Task<List<Customer>> GetAllAsync();
        Task<ErrorOr<Customer>> GetCustomerAsync(Guid id);
        Task<ErrorOr<Customer>> UpdateCustomerCredentialsAsync(Customer customer);
        Task<Updated> UpdateCustomerInfoAsync(Customer customer);
        Task<Updated> UpdateCustomerPasswordAsync(Customer customer);
        Task<Updated> UpdateCustomerStatisticsAsync(Customer customer);
        Task<Deleted> DeleteCustomerAsync(Guid id);
        Task<ErrorOr<string>> CompareCustomerOldPasswordWithExistingOne(Guid id, string oldPassword);

        ErrorOr<Customer> From(CreateCustomerRequest request);
        ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerInfoRequest request);
        ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerCredentialsRequest request);
        ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerPasswordRequest request);
    }
}
