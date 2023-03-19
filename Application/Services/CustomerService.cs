using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using ErrorOr;
using CarSharingApp.Application.ServiceErrors;
using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Services
{
    public sealed class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<ActionNote> _noteRepository;

        public CustomerService(IRepository<Customer> customerRepository,
                               IRepository<ActionNote> noteRepository)
        {
            _customerRepository = customerRepository;
            _noteRepository = noteRepository;
        }

        public async Task<ErrorOr<Customer>> CreateCustomerAsync(Customer customer)
        {
            ErrorOr<Customer> checkCredentialsForUniquenessRequest = await CredentialsCheckout(customer);

            if (checkCredentialsForUniquenessRequest.IsError)
            {
                return checkCredentialsForUniquenessRequest.Errors;
            }

            await _customerRepository.CreateAsync(customer);
            await _noteRepository.CreateAsync(ActionNote.RegisteredNote(customer.Id));

            return customer;
        }

        public async Task<Deleted> DeleteCustomerAsync(Guid id)
        {
            await _customerRepository.DeleteAsync(id);

            var notesToDelete = await _noteRepository.GetAllAsync(note => note.ActorId == id);
            foreach (var note in notesToDelete)
            {
                await _noteRepository.DeleteAsync(note.Id);
            }

            return Result.Deleted;
        }

        public async Task<List<Customer>> GetAllAsync()
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

        public async Task<ErrorOr<Customer>> UpdateCustomerCredentialsAsync(Customer customer)
        {
            ErrorOr<Customer> checkCredentialsForUniquenessRequest = await CredentialsCheckout(customer);

            if (checkCredentialsForUniquenessRequest.IsError)
            {
                return checkCredentialsForUniquenessRequest.Errors;
            }

            await _customerRepository.UpdateAsync(customer);
            await _noteRepository.CreateAsync(ActionNote.UpdatedCustomerCredentialsNote(customer.Id));

            return customer;
        }

        public async Task<Updated> UpdateCustomerInfoAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
            await _noteRepository.CreateAsync(ActionNote.UpdatedCustomerInfoNote(customer.Id));

            return Result.Updated;
        }

        public async Task<Updated> UpdateCustomerPasswordAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
            await _noteRepository.CreateAsync(ActionNote.UpdatedCustomerPasswordNote(customer.Id));

            return Result.Updated;
        }

        public async Task<Updated> UpdateCustomerStatisticsAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);

            return Result.Updated;
        }

        public async Task<ErrorOr<string>> CompareCustomerOldPasswordWithExistingOne(Guid id, string oldPassword)
        {
            Customer? customer = await _customerRepository.GetAsync(id);

            if (customer != null)
            {
                ErrorOr<Credentials> requestToCredetialsResult = Credentials.Create(customer.Credentials.Login, customer.Credentials.Email, oldPassword);

                if (requestToCredetialsResult.IsError)
                    return requestToCredetialsResult.Errors;

                Credentials oldCredentials = requestToCredetialsResult.Value;

                if (customer.Credentials.Password != oldCredentials.Password)
                    return ApplicationErrors.Customer.PasswordConfirmationFailed;
                else
                    return oldPassword;

            }
            else
                return ApplicationErrors.Customer.NotFound;
        }

        public ErrorOr<Customer> From(CreateCustomerRequest request)
        {
            return Customer.Create(
                request.FirstName,
                request.LastName,
                request.StreetAddress,
                request.AptSuiteEtc,
                request.City,
                request.Country,
                request.ZipPostCode,
                request.PhoneNumber,
                request.DriverLicenseIdentifier,
                request.HasAcceptedNewsSharing,
                request.Login,
                request.Email,
                request.Password);
        }

        public ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerInfoRequest request)
        {
            return Customer.Create(
                request.FirstName,
                request.LastName,
                existingCustomer.Address.StreetAddress,
                existingCustomer.Address.AptSuiteEtc,
                existingCustomer.Address.City,
                existingCustomer.Address.Country.Name, 
                existingCustomer.Address.ZipPostCode,
                request.PhoneNumber,
                request.DriverLicenseIdentifier,
                request.HasAcceptedNewsSharing,
                existingCustomer.Credentials.Login,
                existingCustomer.Credentials.Email,
                existingCustomer.Credentials.Password,
                existingCustomer.Id,
                request.ProfileDescription,
                request.ProfileImage,
                existingCustomer.Statistics.VehiclesOrdered,
                existingCustomer.Statistics.VehiclesShared,
                requirePasswordEncryption: false);
        }

        public ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerCredentialsRequest request)
        {
            return Customer.Create(
                existingCustomer.FirstName,
                existingCustomer.LastName,
                existingCustomer.Address.StreetAddress,
                existingCustomer.Address.AptSuiteEtc,
                existingCustomer.Address.City,
                existingCustomer.Address.Country.ToString(),
                existingCustomer.Address.ZipPostCode,
                existingCustomer.PhoneNumber,
                existingCustomer.DriverLicenseIdentifier,
                existingCustomer.HasAcceptedNewsSharing,
                request.Login,
                request.Email,
                existingCustomer.Credentials.Password,
                existingCustomer.Id,
                existingCustomer.Profile.Description,
                existingCustomer.Profile.Image,
                existingCustomer.Statistics.VehiclesOrdered,
                existingCustomer.Statistics.VehiclesShared,
                requirePasswordEncryption: false);
        }

        public ErrorOr<Customer> From(Customer existingCustomer, UpdateCustomerPasswordRequest request)
        {
            return Customer.Create(
                existingCustomer.FirstName,
                existingCustomer.LastName,
                existingCustomer.Address.StreetAddress,
                existingCustomer.Address.AptSuiteEtc,
                existingCustomer.Address.City,
                existingCustomer.Address.Country.ToString(),
                existingCustomer.Address.ZipPostCode,
                existingCustomer.PhoneNumber,
                existingCustomer.DriverLicenseIdentifier,
                existingCustomer.HasAcceptedNewsSharing,
                existingCustomer.Credentials.Login,
                existingCustomer.Credentials.Email,
                request.newPassword,
                existingCustomer.Id,
                existingCustomer.Profile.Description,
                existingCustomer.Profile.Image,
                existingCustomer.Statistics.VehiclesOrdered,
                existingCustomer.Statistics.VehiclesShared);
        }

        private async Task<ErrorOr<Customer>> CredentialsCheckout(Customer customer)
        {
            Customer? customerWithSameLogin = await _customerRepository.GetAsync(c => c.Credentials.Login == customer.Credentials.Login);
            Customer? customerWithSameEmail = await _customerRepository.GetAsync(c => c.Credentials.Email == customer.Credentials.Email);

            switch (customerWithSameEmail, customerWithSameLogin)
            {
                case (customerWithSameEmail: not null, customerWithSameLogin: null):
                    return ApplicationErrors.Customer.EmailHasAlreadyExist;

                case (customerWithSameEmail: null, customerWithSameLogin: not null):
                    return ApplicationErrors.Customer.LoginHasAlreadyExist;

                case (customerWithSameEmail: not null, customerWithSameLogin: not null):
                    return ApplicationErrors.Customer.LoginAndEmailHaveAlreadyExist;

                default:
                    break;
            }

            return customer;
        }
    }
}
