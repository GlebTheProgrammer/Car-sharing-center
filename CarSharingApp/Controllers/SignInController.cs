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
        private readonly IClientsRepository clientsRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;

        public SignInController(IClientsRepository clientsRepository, IHttpContextAccessor httpContextAccessor, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            this.clientsRepository = clientsRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.currentUserStatusProvider = currentUserStatusProvider;
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

            var signedUser = clientsRepository.TrySignIn(clientSignInViewModel.Email, clientSignInViewModel.Password);

            if (signedUser == null)
            {
                httpContextAccessor.HttpContext.Session.SetString("AuthorizationFailed", "true");

                return RedirectToAction("Index");
            }

            // Set signedUser credentials as client (passing role and id)
            if(signedUser.Role == Role.Client)
            {
                currentUserStatusProvider.SetUserCredentials(signedUser.Id, UserRole.Client);
            }

            return RedirectToAction("Index", "CarSharing");
        }

        public void Logout()
        {
            currentUserStatusProvider.SetUserCredentials(null, UserRole.Unauthorized);
        }
    }
}
