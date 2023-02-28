using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Models;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using CarSharingApp.Application.Contracts.Rental;

namespace CarSharingApp.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IAccountServicePublicApiClient _accountServiceClient;
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;
        private readonly IRentalServicePublicApiClient _rentalServiceClient;
        private readonly IAzureBlobStoragePublicApiClient _blobStorageClient;

        public DashboardController(IAccountServicePublicApiClient accountServiceClient, 
                                   IVehicleServicePublicApiClient vehicleServiceClient,
                                   IRentalServicePublicApiClient rentalServiceClient,
                                   IAzureBlobStoragePublicApiClient blobStorageClient)
        {
            _accountServiceClient = accountServiceClient;
            _vehicleServiceClient = vehicleServiceClient;
            _rentalServiceClient = rentalServiceClient;
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

        #region Partial views rendering

        public async Task<IActionResult> RentalsArticlePartial(string searchBy, string searchInput, int page = 1, int pageSize = 3)
        {
            var response = await _accountServiceClient.GetCustomerRentalsAccountRepresentation();

            response.EnsureSuccessStatusCode();

            AccountRentalsDataResponse responseModel = await response.Content.ReadFromJsonAsync<AccountRentalsDataResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            ViewBag.SearchBy = searchBy;
            ViewBag.SearchRentalInput = searchInput;

            if (page < 1)
                page = 1;

            if (searchBy == "Amount" && searchInput != null)
            {
                var partialViewModel = responseModel.Rentals.Where(r => r.Amount.StartsWith(searchInput)).ToList();

                Pager pager = new Pager(partialViewModel.Count(), page, pageSize);
                int rentalsSkip = (page - 1) * pageSize;
                ViewBag.Pager = pager;
                ViewBag.RentalsCount = partialViewModel.Count();

                return PartialView("_RentalsArticle", partialViewModel.Skip(rentalsSkip).Take(pager.PageSize).ToList());
            }
            else if (searchBy == "VehicleName" && searchInput != null)
            {
                var partialViewModel = responseModel.Rentals.Where(r => r.VehicleName.StartsWith(searchInput)).ToList();

                Pager pager = new Pager(partialViewModel.Count(), page, pageSize);
                int rentalsSkip = (page - 1) * pageSize;
                ViewBag.Pager = pager;
                ViewBag.RentalsCount = partialViewModel.Count();

                return PartialView("_RentalsArticle", partialViewModel.Skip(rentalsSkip).Take(pager.PageSize).ToList());
            }
            else
            {
                Pager pager = new Pager(responseModel.Rentals.Count(), page, pageSize);
                int rentalsSkip = (page - 1) * pageSize;
                ViewBag.Pager = pager;
                ViewBag.RentalsCount = responseModel.Rentals.Count();

                return PartialView("_RentalsArticle", responseModel.Rentals.Skip(rentalsSkip).Take(pager.PageSize).ToList());
            }
        }

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

        #endregion

        #region Partial views actions

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

        public async Task<IActionResult> FinishRental(string rentalId, string searchBy, string searchInput)
        {
            var requestModel = new FinishExistingRentalRequest(rentalId);

            var response = await _rentalServiceClient.FinishRentalRequest(requestModel);
            response.EnsureSuccessStatusCode();

            HttpContext.Session.SetString("FinishedRental", "True");

            return Redirect($"/Dashboard/RentalsArticlePartial?searchBy={searchBy}&searchInput={searchInput}");
        }

        [HttpGet]
        public async Task<JsonResult> CheckIfVehicleExists(string id)
        {
            var response = await _vehicleServiceClient.GetVehicleInformation(Guid.Parse(id));

            response.EnsureSuccessStatusCode();

            return Json(new { data = true });
        }

        #endregion
    }
}
