using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Web.Clients;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ICustomerServicePublicApiClient _customerServiceClient;

        public RegistrationController(ICustomerServicePublicApiClient customerServiceClient)
        {
            _customerServiceClient = customerServiceClient;
        }

        public IActionResult Index()
        {
            var createCustomerRequest = new CreateCustomerRequest(
                FirstName: string.Empty,
                LastName: string.Empty,
                StreetAddress: string.Empty,
                AptSuiteEtc: string.Empty,
                City: string.Empty,
                Country: string.Empty,
                ZipPostCode: string.Empty,
                PhoneNumber: string.Empty,
                DriverLicenseIdentifier: string.Empty,
                HasAcceptedNewsSharing: false,
                Login: string.Empty,
                Email: string.Empty,
                Password: string.Empty);

            return View(createCustomerRequest);
        }

        public async Task<IActionResult> Register(CreateCustomerRequest createCustomerRequest)
        {
            var requestResult = await _customerServiceClient.CreteNewCustomer(createCustomerRequest);

            HttpContext.Session.SetString("Registered", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
