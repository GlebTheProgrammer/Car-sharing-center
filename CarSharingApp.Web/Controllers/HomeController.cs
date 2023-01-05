using CarSharingApp.Models;
using CarSharingApp.Web.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public HomeController(IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
        }

        public async Task<IActionResult> Index()
        {

            var response = await _vehicleServiceClient.GetAllApprovedAndPublishedVehiclesMapRepresentation();

















            //List<Vehicle> activeNotRentedVehicles = await _mongoDbService.GetPublishedAndNotOrderedVehicles();
            //int vehiclesCount = activeNotRentedVehicles.Count;

            //float[][] vehiclesLocation = new float[vehiclesCount][];

            //int i = 0;
            //foreach (Vehicle vehicle in activeNotRentedVehicles)
            //{
            //    string latitude = vehicle.Location.Latitude.Replace('.', ',');
            //    string longitude = vehicle.Location.Longitude.Replace('.', ',');
            //    vehiclesLocation[i] = new float[2] {float.Parse(latitude), float.Parse(longitude)};
            //    i += 1;
            //}

            //VehiclesHomeDataViewModel viewModel = new VehiclesHomeDataViewModel()
            //{
            //    Vehicles = _mapper.Map<List<VehicleHomeModel>>(activeNotRentedVehicles),
            //    VehiclesLocation= vehiclesLocation
            //};

            return View(/*viewModel*/);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}