using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using ErrorOr;
using CarSharingApp.Application.ServiceErrors;
using CarSharingApp.Application.Contracts.Customer;

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
            Customer? customerWithSameLogin = await _customerRepository.GetAsync(c => c.Credentials.Login == customer.Credentials.Login);
            Customer? customerWithSameEmail = await _customerRepository.GetAsync(c => c.Credentials.Email == customer.Credentials.Email);

            switch (customerWithSameEmail, customerWithSameLogin)
            {
                case (customerWithSameEmail: not null, customerWithSameLogin: null):
                    return ApplicationErrors.Customer.EmailHasAlreadyExist;

                case (customerWithSameEmail: null, customerWithSameLogin: not null):
                    return ApplicationErrors.Customer.UsernameHasAlreadyExist;

                case (customerWithSameEmail: not null, customerWithSameLogin: not null):
                    return ApplicationErrors.Customer.UsernameAndEmailHaveAlreadyExist;

                default:
                    break;
            }

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

        public ErrorOr<Customer> From(Guid id, UpdateCustomerRequest request)
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
                request.Login,
                request.Email,
                request.Password,
                id,
                request.ProfileDescription,
                request.ProfileImage,
                request.VehiclesOrdered,
                request.VehiclesShared,
                request.IsOnline);
        }
    }
}
