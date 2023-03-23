using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    [AllowAnonymous]
    [Route("registration")]
    public sealed class RegistrationController : Controller
    {
        private readonly ICustomerServicePublicApiClient _customerServiceClient;

        public RegistrationController(ICustomerServicePublicApiClient customerServiceClient)
        {
            _customerServiceClient = customerServiceClient;
        }

        [HttpGet]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public async Task<IActionResult> Register([FromForm] CreateCustomerRequest createCustomerRequest)
        {
            if (!ModelState.IsValid)
                return View("Index", createCustomerRequest);

            var response = await _customerServiceClient.CreateNewCustomer(createCustomerRequest);

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
