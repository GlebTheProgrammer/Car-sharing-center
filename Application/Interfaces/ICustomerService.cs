using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ErrorOr<Created>> CreateCustomerAsync(Customer customer);
        Task<ErrorOr<List<Customer>>> GetAllAsync();
        Task<ErrorOr<Customer>> GetCustomerAsync(Guid id);
        Task<ErrorOr<Updated>> UpdateCustomerAsync(Customer customer);
        Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid id);

        ErrorOr<Customer> From(CreateCustomerRequest request);
        ErrorOr<Customer> From(Guid id, Credentials credentials, UpdateCustomerRequest request);
    }
}
