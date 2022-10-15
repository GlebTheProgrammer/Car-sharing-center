using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
