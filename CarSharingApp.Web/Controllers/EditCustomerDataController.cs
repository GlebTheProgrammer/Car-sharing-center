using CarSharingApp.Application.Contracts.Customer;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [Authorize]
    [Route("edit/information")]
    public sealed class EditCustomerDataController : Controller
    {
        private readonly ICustomerServicePublicApiClient _customerServiceClient;

        public EditCustomerDataController(ICustomerServicePublicApiClient customerServiceClient)
        {
            _customerServiceClient = customerServiceClient;
        }

        public async Task<IActionResult> Index()
        {

            var response = await _customerServiceClient.GetCustomerInformation();

            response.EnsureSuccessStatusCode();

            CustomerResponse responseModel = await response.Content.ReadFromJsonAsync<CustomerResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return View(responseModel);
        }

        //public async Task<IActionResult> EditCustomerData(CustomerEditModel customerEditModel)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Index", customerEditModel);

        //    await _mongoDbService.EditCustomerData(customerEditModel);

        //    HttpContext.Session.SetString("ChangedAccountData", "true");

        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(string newPassword)
        //{
        //    string userId = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("JWToken")).Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        //    await _mongoDbService.ChangePassword(userId, newPassword);

        //    HttpContext.Session.SetString("ChangedPassword", "true");

        //    return RedirectToAction("Index");
        //}
    }
}
