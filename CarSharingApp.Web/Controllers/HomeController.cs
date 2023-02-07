using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Models;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public HomeController(IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesMapRepresentation();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return RedirectToAction("Unauthorized401Error", "CustomExceptionHandle");

                default:
                    break;
            }

            response.EnsureSuccessStatusCode();

            VehiclesDisplayOnMapResponse responseModel = await response.Content.ReadFromJsonAsync<VehiclesDisplayOnMapResponse>() 
                ?? throw new NullReferenceException(nameof(responseModel));

            var viewModel = responseModel.Vehicles;

            return View(viewModel);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}