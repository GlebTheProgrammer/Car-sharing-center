using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Controllers
{
    public class SearchNearbyVehiclesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
