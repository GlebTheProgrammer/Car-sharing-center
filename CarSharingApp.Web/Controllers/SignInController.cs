using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly IAuthorizationServicePublicApiClient _authorizationServiceClient;
        private readonly IAzureADPublicApiClient _azureAdClient;

        public SignInController(IAuthorizationServicePublicApiClient authorizationServiceClient, IAzureADPublicApiClient azureAdClient)
        {
            _authorizationServiceClient = authorizationServiceClient;
            _azureAdClient = azureAdClient;
        }

        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            var authorizationRequest = new AuthorizationRequest
            {
                EmailOrLogin = string.Empty,
                Password = string.Empty
            };

            return View(authorizationRequest);
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> TrySignIn(AuthorizationRequest request)
        //{
        //    var response = await _authorizationServiceClient.TryAuthorize(request);

        //    switch (response.StatusCode)
        //    {
        //        case HttpStatusCode.ServiceUnavailable:
        //        case HttpStatusCode.Forbidden:
        //            {
        //                string responseContent = await response.Content.ReadAsStringAsync();

        //                ValidationError validationError = JsonSerializer.Deserialize<ValidationError>(responseContent) ?? new ValidationError();

        //                ModelState.AddModelError(nameof(request.Password), validationError.Title);

        //                return View("Index", request);
        //            }
        //        default:
        //            break;
        //    }

        //    SuccessfulAuthorizationResponse responseModel = await response.Content.ReadFromJsonAsync<SuccessfulAuthorizationResponse>()
        //        ?? throw new NullReferenceException(nameof(responseModel));

        //    string resultUri = _azureAdClient.RequestAuthorizationCode();

        //    return Redirect(resultUri);
        //}

        //public void AuthenticationCode(string code, string state)
        //{
        //    _azureAdClient.RequestAccessToken(code, state);

        //    return;
        //}

        //public IActionResult Token(string access_token, string token_type, int expires_in, string scope, string refresh_token, string id_token)
        //{
        //    if (access_token == null || token_type == null || expires_in == 0 || scope == null || refresh_token == null || id_token == null)
        //    {
        //        return RedirectToAction("TrySignIn");
        //    }

        //    return View();
        //}


        //[Authorize]
        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync();

        //    HttpContext.Session.SetString("LoggedOut", "true");

        //    return RedirectToAction("Index", "SignIn");
        //}

        public async Task<IActionResult> TrySignIn(AuthorizationRequest request)
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
