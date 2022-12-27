using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using ErrorOr;
using CarSharingApp.Application.ServiceErrors;
using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ErrorOr<Created>> CreateCustomerAsync(Customer customer)
        {
            await _customerRepository.CreateAsync(customer);

            return Result.Created;
        }

        public async Task<ErrorOr<Deleted>> DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);

            return Result.Deleted;
        }

        public async Task<ErrorOr<List<Customer>>> GetAllAsync()
        {
            var result = await _customerRepository.GetAllAsync();

            return result.ToList();
        }

        public async Task<ErrorOr<Customer>> GetCustomerAsync(Guid id)
        {
            var result = await _customerRepository.GetAsync(id);

            if (result != null)
                return result;
            else
                return ApplicationErrors.Customer.NotFound;
        }

        public async Task<ErrorOr<Updated>> UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);

            return Result.Updated;
        }

        public ErrorOr<Customer> From(CreateCustomerRequest request)
        {
            return Customer.Create(
                request.FirstName,
                request.LastName,
                request.Country,
                request.City,
                request.Address,
                request.PhoneNumber,
                request.DriverLicenseIdentifier,
                request.Postcode,
                request.HasAcceptedNewsSharing,
                request.login,
                request.email,
                request.password);
        }

        public ErrorOr<Customer> From(Guid id, Credentials credentials, UpdateCustomerRequest request)
        {
            return Customer.Create(
                request.FirstName,
                request.LastName,
                request.Country,
                request.City,
                request.Address,
                request.PhoneNumber,
                request.DriverLicenseIdentifier,
                request.Postcode,
                request.HasAcceptedNewsSharing,
                credentials.Login,
                credentials.Email,
                credentials.Password,
                id,
                request.ProfileDescription,
                request.ProfileImage,
                request.VehiclesOrdered,
                request.VehiclesShared,
                request.isOnline);
        }
    }
}
