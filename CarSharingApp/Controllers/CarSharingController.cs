using CarSharingApp.Models;
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
                    HourPrice = 20m,
                    DailyPrice = 160m,
                    Image = "../../../Images/BMW_x6.jpg",
                    Description = "Very fast car Very fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast car"

                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    HourPrice = 30m,
                    DailyPrice = 260m,
                    Image = "../../../Images/BMW_x6.jpg",
                    Description = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    HourPrice = 30m,
                    DailyPrice = 360m,
                    Image = "../../../Images/BMW_x6.jpg",
                    Description = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    HourPrice = 40m,
                    DailyPrice = 460m,
                    Image = "../../../Images/BMW_x6.jpg",
                    Description = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Name = "BMW x6",
                    HourPrice = 50m,
                    DailyPrice = 560m,
                    Image = "../../../Images/BMW_x6.jpg",
                    Description = "Very fast car"
                }
            };

            return View(vehicles);
        }

        public string Hello()
        {
            return "Who's there?";
        }
    }
}
