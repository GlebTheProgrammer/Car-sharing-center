using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Models;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Helpers;
using CarSharingApp.Web.Helpers.Models;
using CarSharingApp.Web.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarSharingApp.Controllers
{
    [AllowAnonymous]
    public sealed class HomeController : WebAppController
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public HomeController(IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesMapRepresentation();

            if (!response.IsSuccessStatusCode)
            {
                MyControllerContext context = GenerateControllerContext();
                var errorResponseViewModel = MyCustomResponseAnalyzer.Analize(myControllerContext: context,
                                                                        response: response,
                                                                        onErrorStatusCode_ViewName: "Index",
                                                                        onErrorStatusCode_ViewModel: new List<VehicleDisplayOnMap>());

                return errorResponseViewModel ?? throw new NullReferenceException(nameof(errorResponseViewModel));
            }

            VehiclesDisplayOnMapResponse responseModel = await response.Content.ReadFromJsonAsync<VehiclesDisplayOnMapResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            var viewModel = responseModel.Vehicles;

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}