using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface IAuthorizationService
    {
        Task<ErrorOr<Customer>> TryLogin(Credentials possibleCredentials);

        ErrorOr<Credentials> From(AuthorizationRequest request);
    }
}
