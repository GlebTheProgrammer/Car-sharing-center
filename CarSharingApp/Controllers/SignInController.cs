using CarSharingApp.Login;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.ClientData.Includes;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserStatusProvider _currentUserStatusProvider;
        private readonly IJwtProvider _jwtProvider;

        public SignInController(IRepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, 
                                ICurrentUserStatusProvider currentUserStatusProvider, IJwtProvider jwtProvider)
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
            _currentUserStatusProvider = currentUserStatusProvider;
            _jwtProvider = jwtProvider;
        }

        public IActionResult Index()
        {
            var user = new ClientSignInViewModel();
            return View(user);
        }

        public IActionResult TrySignIn(ClientSignInViewModel clientSignInViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", clientSignInViewModel);
            }

            var signedClient = _repositoryManager.ClientsRepository.TrySignIn(clientSignInViewModel.Email, clientSignInViewModel.Password);

            if (signedClient == null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("AuthorizationFailed", "true");

                return RedirectToAction("Index");
            }

            if (signedClient.Role == Role.Client)
            {
                _currentUserStatusProvider.SetUserCredentials(signedClient.Id, UserRole.Client);
            }
            _currentUserStatusProvider.ChangeSignedInState(true);

            var tokenResult = _jwtProvider.Generate(signedClient);

            if (tokenResult == null)
            {
                throw new Exception("Token can not be generated");
            }

            HttpContext.Session.SetString("JWToken", tokenResult);

            return RedirectToAction("Index", "Home");
        }

        public void Logout()
        {
            _currentUserStatusProvider.SetUserCredentials(null, UserRole.Unauthorized);
            _currentUserStatusProvider.ChangeLoggedOutState(true);
        }
    }
}
