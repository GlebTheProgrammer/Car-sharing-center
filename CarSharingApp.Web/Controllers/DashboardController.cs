using CarSharingApp.Application.Contracts.Account;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Models;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace CarSharingApp.Web.Controllers
{
    [Authorize]
    [Route("dashboard")]
    public sealed class DashboardController : Controller
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

        [HttpGet]
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

        [HttpGet]
        [Route("rentalsPartial")]
        public async Task<IActionResult> RentalsArticlePartial([FromQuery] string searchBy, 
                                                               [FromQuery] string searchInput, 
                                                               [FromQuery] int page = 1, 
                                                               [FromQuery] int pageSize = 3)
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

        [HttpGet]
        [Route("vehiclesPartial")]
        public async Task<IActionResult> VehiclesArticlePartial([FromQuery] string searchBy, 
                                                                [FromQuery] string searchInput, 
                                                                [FromQuery] int page = 1, 
                                                                [FromQuery] int pageSize = 3)
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

        [HttpGet]
        [Route("notesPartial")]
        public async Task<IActionResult> NotesArticlePartial([FromQuery] string type)
        {
            var response = await _accountServiceClient.GetActionNotesOfTheSpecificType(type);

            response.EnsureSuccessStatusCode();

            AccountActionNotesResponse responseModel = await response.Content.ReadFromJsonAsync<AccountActionNotesResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return PartialView("_NotesArticle", responseModel);
        }

        [HttpGet]
        [Route("statisticsPartial")]
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

        [HttpGet]
        [Route("deleteAndRenderPartial/vehicle")]
        public async Task<IActionResult> DeleteVehicleAndRenderVehiclesArticlePartial([FromQuery] string vehicleId, 
                                                                                      [FromQuery] string vehicleImage, 
                                                                                      [FromQuery] string searchBy, 
                                                                                      [FromQuery] string searchInput)
        {
            var response = await _vehicleServiceClient.DeleteVehicle(vehicleId);
            response.EnsureSuccessStatusCode();

            await _blobStorageClient.DeleteBlobAsync(vehicleImage);

            return Redirect($"/dashboard/vehiclesPartial?searchBy={searchBy}&searchInput={searchInput}");
        }

        [HttpGet]
        [Route("updateAndRenderPartial/vehicle/status")]
        public async Task<IActionResult> UpdateVehicleStatus([FromQuery] string vehicleId,
                                                             [FromQuery] string isConfirmedByAdmin, 
                                                             [FromQuery] string isPublished, 
                                                             [FromQuery] string isOrdered,
                                                             [FromQuery] string searchBy, 
                                                             [FromQuery] string searchInput)
        {
            var requestModel = new UpdateVehicleStatusRequest(
                bool.Parse(isOrdered),
                bool.Parse(isPublished),
                bool.Parse(isConfirmedByAdmin));

            var response = await _vehicleServiceClient.UpdateVehicleStatus(Guid.Parse(vehicleId), requestModel);
            response.EnsureSuccessStatusCode();

            return Redirect($"/dashboard/vehiclesPartial?searchBy={searchBy}&searchInput={searchInput}");
        }

        [HttpGet]
        [Route("finishAndRenderPartial/rental")]
        public async Task<IActionResult> FinishRental([FromQuery] string rentalId, 
                                                      [FromQuery] string searchBy, 
                                                      [FromQuery] string searchInput)
        {
            var response = await _rentalServiceClient.FinishRentalRequest(Guid.Parse(rentalId));
            response.EnsureSuccessStatusCode();

            HttpContext.Session.SetString("FinishedRental", "True");

            return Redirect($"/dashboard/rentalsPartial?searchBy={searchBy}&searchInput={searchInput}");
        }

        [HttpGet]
        [Route("checkAvailability/vehicle")]
        public async Task<JsonResult> CheckIfVehicleExists([FromQuery] string id)
        {
            var response = await _vehicleServiceClient.GetVehicleInformation(Guid.Parse(id));

            response.EnsureSuccessStatusCode();

            return Json(new { data = true });
        }

        #endregion
    }
}
