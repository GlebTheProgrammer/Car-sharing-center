using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
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
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.Forbidden:
                    {
                        ModelState.AddModelError(nameof(request.Password), responseContent);

                        return View("Index", request);
                    }
                default:
                    break;
            }

            if (responseContent is null)
                throw new Exception("Token wasn't generated.");

            HttpContext.Session.SetString("JWToken", responseContent);
            HttpContext.Session.SetString("SignedIn", "true");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWToken");
            HttpContext.Session.SetString("LoggedOut", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
