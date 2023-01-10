using CarSharingApp.Application.Contracts.Authorization;
using CarSharingApp.Models.Mongo;
using CarSharingApp.Models.MongoView;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


            //if (!ModelState.IsValid)
            //    return View("Index", signedUser);

            //Customer customer = await _mongoDbService.TrySignIn(signedUser);

            //if (customer == null)
            //{
            //    HttpContext.Session.SetString("AuthorizationFailed", "true");
            //    return RedirectToAction("Index");
            //}

            //Credentials credentials = await _mongoDbService.GetCredetnialsByUserId(customer.Id);

            //var tokenResult = _jwtProvider.Generate(customer, credentials);

            //if (tokenResult == null)
            //    throw new Exception("Token can not be generated");

            //HttpContext.Session.SetString("JWToken", tokenResult);
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
