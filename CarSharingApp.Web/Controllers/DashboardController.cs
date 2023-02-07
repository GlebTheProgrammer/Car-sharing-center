using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Models;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarSharingApp.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAccountServicePublicApiClient _accountServiceClient;
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;
        private readonly IAzureBlobStoragePublicApiClient _blobStorageClient;

        public DashboardController(IAccountServicePublicApiClient accountServiceClient, 
                                   IVehicleServicePublicApiClient vehicleServiceClient,
                                   IAzureBlobStoragePublicApiClient blobStorageClient)
        {
            _accountServiceClient = accountServiceClient;
            _vehicleServiceClient = vehicleServiceClient;
            _blobStorageClient = blobStorageClient;
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

        public async Task<IActionResult> VehiclesArticlePartial(string searchBy, string searchInput, int page = 1, int pageSize = 3)
        {
            var response = await _accountServiceClient.GetCustomerVehiclesAccountRepresentation();

            response.EnsureSuccessStatusCode();

            AccountVehiclesDataResponse responseModel = await response.Content.ReadFromJsonAsync<AccountVehiclesDataResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            ViewBag.SearchBy = searchBy;
            ViewBag.SearchInput = searchInput;

            if (page < 1)
                page = 1;

            if (searchBy == "Price" && searchInput != null)
            {
                var partialViewModel = responseModel.Vehicles.Where(v => v.HourlyPrice.StartsWith(searchInput) || v.DailyPrice.StartsWith(searchInput)).ToList();

                Pager pager = new Pager(partialViewModel.Count(), page, pageSize);
                int vehiclesSkip = (page - 1) * pageSize;
                ViewBag.Pager = pager;
                ViewBag.VehiclesCount = partialViewModel.Count();

                return PartialView("_VehiclesArticle", partialViewModel.Skip(vehiclesSkip).Take(pager.PageSize).ToList());
            } else if (searchBy == "Name" && searchInput != null)
            {
                var partialViewModel = responseModel.Vehicles.Where(v => v.Name.StartsWith(searchInput)).ToList();

                Pager pager = new Pager(partialViewModel.Count(), page, pageSize);
                int vehiclesSkip = (page - 1) * pageSize;
                ViewBag.Pager = pager;
                ViewBag.VehiclesCount = partialViewModel.Count();

                return PartialView("_VehiclesArticle", partialViewModel.Skip(vehiclesSkip).Take(pager.PageSize).ToList());
            }
            else
            {
                Pager pager = new Pager(responseModel.Vehicles.Count(), page, pageSize);
                int vehiclesSkip = (page - 1) * pageSize;
                ViewBag.Pager = pager;
                ViewBag.VehiclesCount = responseModel.Vehicles.Count();

                return PartialView("_VehiclesArticle", responseModel.Vehicles.Skip(vehiclesSkip).Take(pager.PageSize).ToList());
            }
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

        // Partial views actions go below

        public async Task<IActionResult> DeleteVehicleAndRenderVehiclesArticlePartial(string vehicleId, string vehicleImage, string searchBy, string searchInput)
        {
            var response = await _vehicleServiceClient.DeleteVehicle(vehicleId);
            response.EnsureSuccessStatusCode();

            await _blobStorageClient.DeleteBlobAsync(vehicleImage);

            return Redirect($"/Dashboard/VehiclesArticlePartial?searchBy={searchBy}&searchInput={searchInput}");
        }

        public async Task<IActionResult> UpdateVehicleStatus(string vehicleId, string isConfirmedByAdmin, string isPublished, string isOrdered,
                                                             string searchBy, string searchInput)
        {
            var requestModel = new UpdateVehicleStatusRequest(
                vehicleId,
                bool.Parse(isOrdered),
                bool.Parse(isPublished),
                bool.Parse(isConfirmedByAdmin));

            var response = await _vehicleServiceClient.UpdateVehicleStatus(requestModel);
            response.EnsureSuccessStatusCode();

            return Redirect($"/Dashboard/VehiclesArticlePartial?searchBy={searchBy}&searchInput={searchInput}");
        }

    }
}
