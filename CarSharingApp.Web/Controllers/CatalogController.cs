using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public CatalogController(IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Partial views rendering goes below

        public async Task<IActionResult> VehiclesCatalogPartial()
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesCatalogRepresentation();

            response.EnsureSuccessStatusCode();

            VehiclesDisplayInCatalogResponse responseModel = await response.Content.ReadFromJsonAsync<VehiclesDisplayInCatalogResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return PartialView("_VehiclesCatalog", responseModel.Vehicles);
        }
    }
}
