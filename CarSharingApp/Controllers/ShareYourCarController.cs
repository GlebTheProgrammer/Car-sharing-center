using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class ShareYourCarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
