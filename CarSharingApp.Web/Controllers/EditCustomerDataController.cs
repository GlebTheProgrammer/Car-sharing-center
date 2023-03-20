using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [Authorize]
    [Route("customer/current/edit")]
    public sealed class EditCustomerDataController : Controller
    {
        private readonly ICustomerServicePublicApiClient _customerServiceClient;

        public EditCustomerDataController(ICustomerServicePublicApiClient customerServiceClient)
        {
            _customerServiceClient = customerServiceClient;
        }

        [HttpGet]
        [Route("information")]
        public async Task<IActionResult> Index()
        {
            var response = await _customerServiceClient.GetCustomerInformation();

            response.EnsureSuccessStatusCode();

            CustomerDataResponse responseModel = await response.Content.ReadFromJsonAsync<CustomerDataResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return View(responseModel);
        }

        [HttpPost]
        [Route("information")]
        public async Task<IActionResult> Edit([FromForm] CustomerDataResponse request)
        {
            if (!ModelState.IsValid)
                return View("Index", request);

            var response = await _customerServiceClient.EditCustomerInformation(
                MapUpdateCustomerInfoRequest(request));

            HttpContext.Session.SetString("CustomerImage", request.Image);
            HttpContext.Session.SetString("ChangedAccountData", "true");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("password")]
        public async Task<IActionResult> ChangePassword([FromForm] string newPassword)
        {
            var response = await _customerServiceClient.EditCustomerPassword(
                MapUpdateCustomerPasswordRequest(newPassword));

            string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}" + Url.Action("Index", "Dashboard")
                ?? throw new NullReferenceException(nameof(url));

            Response.Headers.Add("Location", url);
            return Redirect(url);
        }

        private UpdateCustomerInfoRequest MapUpdateCustomerInfoRequest(CustomerDataResponse response)
        {
            return new UpdateCustomerInfoRequest(
                response.FirstName,
                response.LastName,
                response.PhoneNumber,
                response.DriverLicenseIdentifier,
                response.HasAcceptedNewsSharing,
                response.ProfileDescription,
                response.Image);
        }

        private UpdateCustomerPasswordRequest MapUpdateCustomerPasswordRequest(string newPassword)
        {
            return new UpdateCustomerPasswordRequest(
                currentPassword: string.Empty,
                newPassword: newPassword);
        }
    }
}
