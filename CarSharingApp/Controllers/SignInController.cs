using CarSharingApp.Login;
using CarSharingApp.Models.Mongo;
using CarSharingApp.Models.MongoView;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly MongoDbService _mongoDbService;
        private readonly IJwtProvider _jwtProvider;

        public SignInController(MongoDbService mongoDbService, IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
            _mongoDbService= mongoDbService;
        }

        public IActionResult Index()
        {
            var unsignedUser = new UserSignInModel();

            return View(unsignedUser);
        }

        public async Task<IActionResult> TrySignIn(UserSignInModel signedUser)
        {
            if (!ModelState.IsValid)
                return View("Index", signedUser);

            Customer customer = await _mongoDbService.TrySignIn(signedUser);

            if (customer == null)
            {
                HttpContext.Session.SetString("AuthorizationFailed", "true");
                return RedirectToAction("Index");
            }

            Credentials credentials = await _mongoDbService.GetCredetnialsByUserId(customer.Id);

            var tokenResult = _jwtProvider.Generate(customer, credentials);

            if (tokenResult == null)
                throw new Exception("Token can not be generated");

            HttpContext.Session.SetString("JWToken", tokenResult);
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
