using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly IAuthorizationServicePublicApiClient _authorizationServiceClient;

        public SignInController(IAuthorizationServicePublicApiClient authorizationServiceClient)
        {
            _authorizationServiceClient = authorizationServiceClient;
        }

        public IActionResult Index()
        {
            var authorizationRequest = new AuthorizationRequest(
                EmailOrLogin: string.Empty,
                Password: string.Empty);

            return View(authorizationRequest);
        }

        public async Task<IActionResult> TrySignIn(AuthorizationRequest request)
        {
            var response = await _authorizationServiceClient.TryAuthorize(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    {
                        ForbiddenError forbiddenError = JsonSerializer.Deserialize<ForbiddenError>(responseContent) ?? new ForbiddenError();

                        ModelState.AddModelError(nameof(request.EmailOrLogin), forbiddenError.Title);

                        return View("Index", request);
                    }
                default:
                    break;
            }

            TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent) ?? new TokenResponse(string.Empty);

            if (tokenResponse.JWToken is null)
                throw new Exception("Token was'nt generated");

            HttpContext.Session.SetString("JWToken", tokenResponse.JWToken);
            HttpContext.Session.SetString("SignedIn", "true");

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.SetString("LoggedOut", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
