using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers.Handle
{
    public class CustomExceptionHandleController : Controller
    {
        public IActionResult Unauthorized401Error()
        {
            return View();
        }
    }
}
