using CarSharingApp.Models.MongoView;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public RegistrationController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public IActionResult Index()
        {
            var unsignedUser = new CustomerRegisterModel();

            return View(unsignedUser);
        }

        public async Task<IActionResult> Register(CustomerRegisterModel newCustomerRegisterModel)
        {
            if (!ModelState.IsValid)
                return View("Index", newCustomerRegisterModel);

            await _mongoDbService.RegisterNewCustomer(newCustomerRegisterModel);

            HttpContext.Session.SetString("Registered", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
