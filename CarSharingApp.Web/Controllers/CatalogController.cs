using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [AllowAnonymous]
    [Route("catalog")]
    public sealed class CatalogController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public CatalogController(IVehicleServicePublicApiClient vehicleServiceClient)
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
        [Route("vehiclesPartial")]
        public async Task<IActionResult> VehiclesCatalogPartial([FromQuery] string searchBy, 
                                                                [FromQuery] string searchInput, 
                                                                [FromQuery] int page = 1, 
                                                                [FromQuery] int pageSize = 9)
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesCatalogRepresentation();
            var viewModel = await RenderCatalog(searchBy, searchInput, page, pageSize, response);

            return PartialView("_VehiclesCatalog", viewModel);
        }

        [HttpGet]
        [Route("advancedFilterPartial")]
        public IActionResult FilterModalPartial()
        {
            return PartialView("_FilterModal");
        }

        #endregion

        #region Partial views actions

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> AdvancedSearch([FromQuery] GetVehiclesByCriteriaRequest request, 
                                                        [FromQuery] string searchBy, 
                                                        [FromQuery] string searchInput, 
                                                        [FromQuery] int page = 1, 
                                                        [FromQuery] int pageSize = 9)
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesWithFilterCatalogRepresentation(request);
            var viewModel = await RenderCatalog(searchBy, searchInput, page, pageSize, response);

            return PartialView("_VehiclesCatalog", viewModel);
        }

        #endregion

        #region Helper methods

        [NonAction]
        private async Task<List<VehicleDisplayInCatalog>> RenderCatalog(string searchBy, string searchInput, int page, int pageSize, HttpResponseMessage response)
        {
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

            return partialViewModel.Skip(vehiclesSkip).Take(pager.PageSize).ToList();
        }

        [NonAction]
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

        #endregion
    }
}
