using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Controllers
{
    [AllowAnonymous]
    [Route("nearbyVehicles")]
    public class SearchNearbyVehiclesController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public SearchNearbyVehiclesController(IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region Partial views rendering 

        [HttpGet]
        [Route("mapPartial")]
        public async Task<IActionResult> RenderMapPartial()
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesMapRepresentation();

            response.EnsureSuccessStatusCode();

            VehiclesDisplayOnMapResponse responseModel = await response.Content.ReadFromJsonAsync<VehiclesDisplayOnMapResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            var viewModel = responseModel.Vehicles;

            return PartialView("_MapArticle", viewModel);
        }

        #endregion

        #region Partial views actions

        [HttpGet]
        [Route("getNearbyVehicles")]
        public async Task<JsonResult> GetNearbyVehicles([FromQuery] string latitude, 
                                                        [FromQuery] string longitude, 
                                                        [FromQuery] int vehiclesCount)
        {
            var requestModel = new GetNearbyVehiclesMapRepresentationRequest(latitude, longitude, vehiclesCount);

            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedNearbyVehiclesMapRepresentation(requestModel);

            response.EnsureSuccessStatusCode();

            NearbyVehiclesDisplayOnMapResponse responseModel = await response.Content.ReadFromJsonAsync<NearbyVehiclesDisplayOnMapResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            var resStr = await response.Content.ReadAsStringAsync();
            return Json(responseModel);
        }

        #endregion
    }
}
