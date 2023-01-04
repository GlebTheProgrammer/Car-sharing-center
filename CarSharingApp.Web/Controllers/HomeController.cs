using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Models;
using CarSharingApp.Web.Client;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            using(System.Net.Http.HttpClient client = HttpClientProvider.Configure())
            {
                var response = await client.GetAsync("https://localhost:44363/Vehicles/MapRepresentation");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var responseDeserialized = JsonSerializer.Deserialize<VehiclesDisplayOnMapResponse>(jsonString);
                    List<VehicleDisplayOnMap> list = responseDeserialized.vehicles;
                }
                else
                {

                }
            }

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