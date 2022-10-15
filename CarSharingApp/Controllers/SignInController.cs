using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
