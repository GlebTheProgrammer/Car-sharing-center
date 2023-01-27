using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarSharingApp.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAccountServicePublicApiClient _accountServiceClient;

        public DashboardController(IAccountServicePublicApiClient accountServiceClient)
        {
            _accountServiceClient = accountServiceClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _accountServiceClient.GetCustomerAccountData();

            var temp = response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return RedirectToAction("Unauthorized401Error", "CustomExceptionHandle");

                default:
                    break;
            }

            response.EnsureSuccessStatusCode();

            AccountDataResponse responseModel = await response.Content.ReadFromJsonAsync<AccountDataResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return View(responseModel);
        }
    }
}
