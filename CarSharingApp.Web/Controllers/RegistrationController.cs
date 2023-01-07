using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Application.Contracts.Error;
using CarSharingApp.Web.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

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
            var response = await _customerServiceClient.CreteNewCustomer(createCustomerRequest);

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        ValidationError validationError = JsonSerializer.Deserialize<ValidationError>(responseContent) ?? new ValidationError();

                        foreach (var error in validationError.Errors)
                        {
                            ModelState.AddModelError(error.Key, error.Value.FirstOrDefault() ?? string.Empty);
                        }

                        return View(createCustomerRequest);
                    }
                default:
                    break;
            }

            HttpContext.Session.SetString("Registered", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
