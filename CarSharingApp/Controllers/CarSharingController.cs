using CarSharingApp.Models.VehicleData;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class CarSharingController : Controller
    {
        public IActionResult Index()
        {
            List<VehicleViewModel> vehicles = new List<VehicleViewModel>()
            {
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 20m, TariffPerDay = 160m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car Very fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast car"

                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 260m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 360m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 40m, TariffPerDay = 460m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 50m, TariffPerDay = 560m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                }
            };

            return View(vehicles);
        }
    }
}
