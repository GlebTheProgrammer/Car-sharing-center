using CarSharingApp.Models;
using CarSharingApp.Models.Mongo;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public HomeController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            //var vehiclesIds = _repositoryManager.OrdersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            //if (vehiclesIds.Count > 0)
            //    _repositoryManager.VehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            List<Vehicle> activeNotOrderedVehicles = await _mongoDbService.GetPublishedAndNotOrderedVehicles();
            int vehiclesCount = activeNotOrderedVehicles.Count;

            float[][] array = new float[vehiclesCount][];

            int i = 0;
            foreach (Vehicle vehicle in activeNotOrderedVehicles)
            {
                string latitude = vehicle.Location.Latitude.Replace('.', ',');
                string longitude = vehicle.Location.Longitude.Replace('.', ',');
                array[i] = new float[2] {float.Parse(latitude), float.Parse(longitude)};
                i += 1;
            }

            return View(array);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}