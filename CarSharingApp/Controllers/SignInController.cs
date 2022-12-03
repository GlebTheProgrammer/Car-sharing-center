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

        public SignInController(IRepositoryManager repositoryManager, IHttpContextAccessor httpContextAccessor, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
            _currentUserStatusProvider = currentUserStatusProvider;
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

            var signedUser = _repositoryManager.ClientsRepository.TrySignIn(clientSignInViewModel.Email, clientSignInViewModel.Password);

            if (signedUser == null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("AuthorizationFailed", "true");

                return RedirectToAction("Index");
            }

            if(signedUser.Role == Role.Client)
            {
                _currentUserStatusProvider.SetUserCredentials(signedUser.Id, UserRole.Client);
            }
            _currentUserStatusProvider.ChangeSignedInState(true);

            return RedirectToAction("Index", "Home");
        }

        public void Logout()
        {
            _currentUserStatusProvider.SetUserCredentials(null, UserRole.Unauthorized);
            _currentUserStatusProvider.ChangeLoggedOutState(true);
        }
    }
}
