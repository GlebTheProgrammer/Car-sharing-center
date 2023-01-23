using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
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

        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            var authorizationRequest = new AuthorizationRequest
            {
                EmailOrLogin = string.Empty,
                Password = string.Empty,
                ReturnUrl = returnUrl
            };

            return View(authorizationRequest);
        }

        [AllowAnonymous]
        public async Task<IActionResult> TrySignIn(AuthorizationRequest request)
        {
            var response = await _authorizationServiceClient.TryAuthorize(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.Forbidden:
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        ValidationError validationError = JsonSerializer.Deserialize<ValidationError>(responseContent) ?? new ValidationError();

                        ModelState.AddModelError(nameof(request.Password), validationError.Title);

                        return View("Index", request);
                    }
                default:
                    break;
            }

            SuccessfulAuthorizationResponse responseModel = await response.Content.ReadFromJsonAsync<SuccessfulAuthorizationResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            var claims = new List<Claim>
            {
                new Claim(type: "sub", value: responseModel.Id),
                new Claim(type: "email", value: responseModel.Email),
                new Claim(type: "username", value: responseModel.Login),
                new Claim(type: "role", value: "Customer"),
            };

            ClaimsIdentity claimsIdentity= new ClaimsIdentity(claims, "pwd", "username", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            HttpContext.Session.SetString("SignedIn", "true");

            return LocalRedirect(request.ReturnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString("LoggedOut", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
