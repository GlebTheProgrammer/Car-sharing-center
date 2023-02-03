using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Models;
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

        public async Task<IActionResult> VehiclesCatalogPartial(string searchBy, string searchInput, int page = 1, int pageSize = 9)
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesCatalogRepresentation();

            response.EnsureSuccessStatusCode();

            VehiclesDisplayInCatalogResponse responseModel = await response.Content.ReadFromJsonAsync<VehiclesDisplayInCatalogResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            ViewBag.SearchBy = searchBy;
            ViewBag.SearchInput = searchInput;

            if (page < 1)
                page = 1;

            List<VehicleDisplayInCatalog> partialViewModel;

            if (searchInput != null)
                partialViewModel = GetViewModel(responseModel, searchBy, searchInput);
            else
                partialViewModel = responseModel.Vehicles;

            Pager pager = new(partialViewModel.Count, page, pageSize);
            int vehiclesSkip = (page - 1) * pageSize;
            ViewBag.Pager = pager;
            ViewBag.VehiclesCount = partialViewModel.Count;

            return PartialView("_VehiclesCatalog", partialViewModel.Skip(vehiclesSkip).Take(pager.PageSize).ToList());
        }

        public IActionResult FilterModalPartial()
        {
            return PartialView("_FilterModal");
        }

        private List<VehicleDisplayInCatalog> GetViewModel(VehiclesDisplayInCatalogResponse serverResponse, string searchCriteria, string searchInput)
        {
            switch (searchCriteria)
            {
                case "Price":
                    return serverResponse.Vehicles.Where(v => v.HourlyRentalPrice.StartsWith(searchInput) || v.DailyRentalPrice.StartsWith(searchInput)).ToList();
                case "Name":
                    return serverResponse.Vehicles.Where(v => v.Name.StartsWith(searchInput)).ToList();
                case "BriefDescription":
                    return serverResponse.Vehicles.Where(v => v.BriefDescription.Contains(searchInput)).ToList();
                default:
                    return new List<VehicleDisplayInCatalog>();
            }
        }
    }
}
