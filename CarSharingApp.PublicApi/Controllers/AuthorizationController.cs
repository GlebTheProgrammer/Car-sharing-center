using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using CarSharingApp.Infrastructure.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public sealed class AuthorizationController : ApiController
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IJwtProvider jwtProvider, IAuthorizationService authorizationService)
        {
            _jwtProvider = jwtProvider;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> GenerateToken(AuthorizationRequest request)
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

            string JWToken = _jwtProvider.Generate(customer);

            return CreatedAtAction(
                actionName: nameof(GenerateToken),
                value: MapTokenResponse(JWToken));
        }

        private static TokenResponse MapTokenResponse(string Token)
        {
            return new TokenResponse(Token);
        }
    }
}
