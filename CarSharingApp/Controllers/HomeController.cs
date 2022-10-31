using CarSharingApp.Models;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace CarSharingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeController(ICurrentUserStatusProvider currentUserStatusProvider, IVehiclesRepository vehiclesRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.currentUserStatusProvider = currentUserStatusProvider;
            this.vehiclesRepository = vehiclesRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            int vehiclesCount = vehiclesRepository.GetAllVehicles().Count();

            float[][] array = new float[vehiclesCount][];

            int i = 0;
            foreach (var vehicle in vehiclesRepository.GetAllVehicles())
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