using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class CarInformationController : Controller
    {


        public IActionResult Index(int id)
        {

            return View();
        }
    }
}
