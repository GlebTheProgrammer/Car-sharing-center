using CarSharingApp.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.IdentityServer.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [Route("[action]")]
        public IActionResult SignIn(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SignIn(LoginViewModel model)
        {
            return View();
        }
    }
}
