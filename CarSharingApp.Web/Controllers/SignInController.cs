using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    [Route("signIn")]
    public sealed class SignInController : Controller
    {
        private readonly IAuthorizationServicePublicApiClient _authorizationServiceClient;

        public SignInController(IAuthorizationServicePublicApiClient authorizationServiceClient)
        {
            _authorizationServiceClient = authorizationServiceClient;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var authorizationRequest = new AuthorizationRequest
            {
                EmailOrLogin = string.Empty,
                Password = string.Empty
            };

            return View(authorizationRequest);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public async Task<IActionResult> TrySignIn([FromForm] AuthorizationRequest request)
        {
            var response = await _authorizationServiceClient.TryAuthorize(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    {
                        ForbiddenError forbiddenError = JsonSerializer.Deserialize<ForbiddenError>(responseContent) ?? new ForbiddenError();

                        ModelState.AddModelError(nameof(request.Password), forbiddenError.Title);

                        return View("Index", request);
                    }
                default:
                    break;
            }

            TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent) ?? throw new NullReferenceException(nameof(tokenResponse));

            if (tokenResponse.JWToken is null)
                throw new Exception("Token was'nt generated");

            HttpContext.Session.SetString("JWToken", tokenResponse.JWToken);
            HttpContext.Session.SetString("CustomerImage", tokenResponse.CustomerImage);

            HttpContext.Session.SetString("SignedIn", "true");

            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize]
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.SetString("LoggedOut", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
