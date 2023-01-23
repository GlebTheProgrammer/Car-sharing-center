using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public sealed class AuthorizationController : ApiController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Authorize(AuthorizationRequest request)
        {
            ErrorOr<Credentials> requestToCredentialsResult = _authorizationService.From(request);

            if (requestToCredentialsResult.IsError)
            {
                return Problem(requestToCredentialsResult.Errors);
            }

            Credentials credentials = requestToCredentialsResult.Value;

            ErrorOr<Customer> loginResult = await _authorizationService.TryLogin(credentials);

            if (loginResult.IsError)
            {
                return Problem(loginResult.Errors);
            }

            Customer customer = loginResult.Value;

            return Ok(value: MapAuthorizationResponse(customer));
        }

        private static SuccessfulAuthorizationResponse MapAuthorizationResponse(Customer customer)
        {
            return new SuccessfulAuthorizationResponse(
                customer.Id.ToString(),
                customer.Credentials.Login,
                customer.Credentials.Email);
        }
    }
}
