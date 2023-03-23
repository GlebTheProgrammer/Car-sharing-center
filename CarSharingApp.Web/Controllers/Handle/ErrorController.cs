using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Controllers.Handle
{
    public sealed class ErrorController : Controller
    {
        [Route("error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 400:
                    ViewBag.ErrorDescription = "Wrong HTTP request was sent to the server or has invalid syntax";
                    return View("BadRequest");

                case 401:
                    ViewBag.ErrorDescription = "Access is allowed only for registered users";
                    return View("Unauthorized");

                default:
                case 404:
                    ViewBag.ErrorDescription = "Sorry but the page you are looking for does not exist, have been removed, name changed or is temporarily unavailable";
                    return View("NotFound");

                case 500:
                    ViewBag.ErrorDescription = "The server encountered an unexpected condition that prevented it from fulfilling the request";
                    return View("InternalServerError");
            }
        }
    }
}
