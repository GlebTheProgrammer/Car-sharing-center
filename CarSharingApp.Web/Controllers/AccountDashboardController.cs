using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Controllers
{
    public class AccountDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
