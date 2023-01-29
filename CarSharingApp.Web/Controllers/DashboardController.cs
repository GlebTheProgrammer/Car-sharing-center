using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Models.ApplicationData;
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

        // Partial views rendering goes below

        public async Task<IActionResult> VehiclesArticlePartial(int page = 1, int pageSize = 3)
        {
            var response = await _accountServiceClient.GetCustomerVehiclesAccountRepresentation();

            response.EnsureSuccessStatusCode();

            AccountVehiclesDataResponse responseModel = await response.Content.ReadFromJsonAsync<AccountVehiclesDataResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            if (page < 1)
                page = 1;

            Pager pager = new Pager(responseModel.Vehicles.Count(), page, pageSize);
            int vehiclesSkip = (page - 1) * pageSize;

            ViewBag.Pager = pager;

            return PartialView("_VehiclesArticle", responseModel.Vehicles.Skip(vehiclesSkip).Take(pager.PageSize).ToList());
        }

        public async Task<IActionResult> NotesArticlePartial(string type)
        {
            var response = await _accountServiceClient.GetActionNotesOfTheSpecificType(type);

            response.EnsureSuccessStatusCode();

            AccountActionNotesResponse responseModel = await response.Content.ReadFromJsonAsync<AccountActionNotesResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return PartialView("_NotesArticle", responseModel);
        }

        public async Task<IActionResult> StatisticsArticlePartial()
        {
            var response = await _accountServiceClient.GetCustomerAccountStatistics();

            response.EnsureSuccessStatusCode();

            AccountStatisticsDataResponse responseModel = await response.Content.ReadFromJsonAsync<AccountStatisticsDataResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return PartialView("_StatisticsArticle", responseModel);
        }
    }
}
