using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ErrorOr<Created>> CreateCustomerAsync(Customer customer);
        Task<ErrorOr<List<Customer>>> GetAllAsync();
        Task<ErrorOr<Customer>> GetCustomerAsync(Guid id);
        Task<ErrorOr<Updated>> UpdateCustomerCredentialsAsync(Customer customer);
        Task<ErrorOr<Updated>> UpdateCustomerInfoAsync(Customer customer);
        Task<ErrorOr<Updated>> UpdateCustomerPasswordAsync(Customer customer);
        Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid id);
        Task<ErrorOr<Success>> CompareCustomerOldPasswordWithExistingOne(Guid id, string oldPassword);

        ErrorOr<Customer> From(CreateCustomerRequest request);
        ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerInfoRequest request);
        ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerCredentialsRequest request);
        ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerPasswordRequest request);
    }
}
