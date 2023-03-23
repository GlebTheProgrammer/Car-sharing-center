using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.ValueObjects;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.PublicApi.Primitives;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    [Route("api/authorization")]
    public sealed class AuthorizationController : ApiController
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IJwtProvider jwtProvider, 
                                       IAuthorizationService authorizationService, 
                                       ILogger<AuthorizationController> logger)
        {
            _jwtProvider = jwtProvider;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Route("jwToken")]
        public async Task<IActionResult> GenerateToken([FromQuery] AuthorizationRequest request)
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
                _logger.LogInformation("Someone has provided wrong credentials and failed the authorization process.");
                return Problem(loginResult.Errors);
            }

            Customer customer = loginResult.Value;

            string JWToken = _jwtProvider.Generate(customer);

            _logger.LogInformation("Server has generated new JWToken for the customer with ID: {customerId}.", customer.Id);

            return CreatedAtAction(
                actionName: nameof(GenerateToken),
                value: MapTokenResponse(JWToken, customer.Profile.Image));
        }

        #region Response mapping section

        [NonAction]
        private static TokenResponse MapTokenResponse(string token, string image)
        {
            return new TokenResponse(
                JWToken: token,
                CustomerImage: image);
        }

        #endregion
    }
}
