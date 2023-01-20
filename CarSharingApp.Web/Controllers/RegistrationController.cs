using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    public sealed class RegistrationController : Controller
    {
        private readonly ICustomerServicePublicApiClient _customerServiceClient;

        public RegistrationController(ICustomerServicePublicApiClient customerServiceClient)
        {
            _customerServiceClient = customerServiceClient;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await _customerServiceClient.GetCreateNewCustomerRequestTemplate();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return RedirectToAction("Unauthorized401Error", "CustomExceptionHandle");

                default:
                    break;
            }

            response.EnsureSuccessStatusCode();

            CreateCustomerRequest request = await response.Content.ReadFromJsonAsync<CreateCustomerRequest>()
                ?? throw new NullReferenceException(nameof(request));

            return View(request);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateCustomerRequest createCustomerRequest)
        {
            var response = await _customerServiceClient.CreteNewCustomer(createCustomerRequest);

            string responseContent = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    {
                        ValidationError validationError = JsonSerializer.Deserialize<ValidationError>(responseContent) ?? new ValidationError();

                        foreach (var error in validationError.Errors)
                        {
                            ModelState.AddModelError(error.Key.Contains('.') ? error.Key.Substring(error.Key.LastIndexOf('.')) : error.Key, 
                                error.Value.FirstOrDefault() ?? string.Empty);
                        }

                        return View("Index", createCustomerRequest);
                    }
                case HttpStatusCode.Conflict:
                    {
                        ValidationError validationError = JsonSerializer.Deserialize<ValidationError>(responseContent) ?? new ValidationError();

                        ViewBag.ConflictErrorMessage = validationError.Title;

                        return View("Index", createCustomerRequest);
                    }
                default:
                    break;
            }

            HttpContext.Session.SetString("Registered", "true");

            return RedirectToAction("Index", "SignIn");
        }
    }
}
