using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
