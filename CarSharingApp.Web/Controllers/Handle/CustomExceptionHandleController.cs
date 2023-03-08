using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers.Handle
{
    public sealed class CustomExceptionHandleController : Controller
    {
        public IActionResult Unauthorized401Error()
        {
            return View();
        }
    }
}
