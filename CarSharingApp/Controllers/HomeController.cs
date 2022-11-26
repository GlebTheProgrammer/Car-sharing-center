using CarSharingApp.Models;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IOrdersRepository ordersRepository;

        public HomeController(IVehiclesRepository vehiclesRepository, IOrdersRepository ordersRepository)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            // Проверка заказов на просроченное время
            var vehiclesIds = ordersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            if (vehiclesIds.Count > 0)
                vehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            int vehiclesCount = vehiclesRepository.GetAllVehiclesForCatalog().Count();

            float[][] array = new float[vehiclesCount][];

            int i = 0;
            foreach (var vehicle in vehiclesRepository.GetAllVehiclesForCatalog())
            {
                string latitude = vehicle.Location.Latitude.Replace('.', ',');
                string longitude = vehicle.Location.Longitude.Replace('.', ',');
                array[i] = new float[2] {float.Parse(latitude), float.Parse(longitude)};
                i += 1;
            }

            return View(array);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}