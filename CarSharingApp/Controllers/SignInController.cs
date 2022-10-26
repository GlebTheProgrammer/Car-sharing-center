using CarSharingApp.Models.ClientData;
using CarSharingApp.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class SignInController : Controller
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SignInController(IClientsRepository clientsRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.clientsRepository = clientsRepository;
            this.httpContextAccessor = httpContextAccessor;
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

            return View();
        }
    }
}
