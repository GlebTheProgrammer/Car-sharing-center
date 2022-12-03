using CarSharingApp.Models;
using CarSharingApp.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;

        public HomeController(IRepositoryManager repositoryManager)
        {
            _repositoryManager= repositoryManager;
        }

        public IActionResult Index()
        {
            var vehiclesIds = _repositoryManager.OrdersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            if (vehiclesIds.Count > 0)
                _repositoryManager.VehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            int vehiclesCount = _repositoryManager.VehiclesRepository.GetAllVehiclesForCatalog().Count();

            float[][] array = new float[vehiclesCount][];

            int i = 0;
            foreach (var vehicle in _repositoryManager.VehiclesRepository.GetAllVehiclesForCatalog())
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