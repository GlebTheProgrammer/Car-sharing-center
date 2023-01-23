using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;

namespace CarSharingApp.Application.Services
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository<Customer> _customerRepository;

        public AuthorizationService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ErrorOr<Customer>> TryLogin(Credentials possibleCredentials)
        {
            var loginResult = await _customerRepository.GetAsync(c => c.Credentials.Password == possibleCredentials.Password
            && (c.Credentials.Email == possibleCredentials.Email || c.Credentials.Login == possibleCredentials.Login));

            if (loginResult != null)
                return loginResult;
            else
                return ServiceErrors.ApplicationErrors.Authorization.Forbidden;
        }

        public ErrorOr<Credentials> From(AuthorizationRequest request)
        {
            return Credentials.CreateForAuthorization(
                request.EmailOrLogin,
                request.EmailOrLogin,
                request.Password);
        }
    }
}
