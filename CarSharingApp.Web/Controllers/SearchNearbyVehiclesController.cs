using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Helpers.Models;
using CarSharingApp.Web.Helpers;
using CarSharingApp.Web.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Web.Controllers
{
    [AllowAnonymous]
    [Route("nearbyVehicles")]
    public sealed class SearchNearbyVehiclesController : WebAppController
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

            if (!response.IsSuccessStatusCode)
            {
                MyControllerContext context = GenerateControllerContext();
                var errorResponseViewModel = MyCustomResponseAnalyzer.Analize(myControllerContext: context,
                                                                              response: response,
                                                                              onErrorStatusCode_ViewName: "_MapArticle",
                                                                              onErrorStatusCode_ViewModel: new List<VehicleDisplayOnMap>(),
                                                                              isReturningPartialView: true);

                return PartialView("_MapArticle", new List<VehicleDisplayOnMap>());
                //return errorResponseViewModel ?? throw new NullReferenceException(nameof(errorResponseViewModel));
            }

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

            if (!response.IsSuccessStatusCode)
            {
                MyControllerContext context = GenerateControllerContext();
                var errorResponseViewModel = MyCustomResponseAnalyzer.Analize<object>(myControllerContext: context,
                                                                                      response: response,
                                                                                      onErrorStatusCode_ViewName: "",
                                                                                      onErrorStatusCode_ViewModel: null);

                return Json(new NearbyVehiclesDisplayOnMapResponse(new List<NearbyVehicleDisplayOnMapResponse>()));
            }

            NearbyVehiclesDisplayOnMapResponse responseModel = await response.Content.ReadFromJsonAsync<NearbyVehiclesDisplayOnMapResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return Json(responseModel);
        }

        #endregion
    }
}
