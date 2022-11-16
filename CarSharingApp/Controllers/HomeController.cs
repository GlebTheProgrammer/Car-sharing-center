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

        public HomeController(ICurrentUserStatusProvider currentUserStatusProvider, IVehiclesRepository vehiclesRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.vehiclesRepository = vehiclesRepository;
        }

        public IActionResult Index()
        {
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