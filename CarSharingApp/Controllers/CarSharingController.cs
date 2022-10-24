using AutoMapper;
using CarSharingApp.Models.ApplicationData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Views.CarSharing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CarSharingApp.Controllers
{
    public class CarSharingController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;


        public CarSharingController(IVehiclesRepository vehiclesRepository, IMapper mapper)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
        }



        public IActionResult Index()
        {
            //var vehicleViewModels = mapper.Map<IEnumerable<VehicleViewModel>>(vehiclesRepository.GetAllVehicles()).Take(3).ToList();

            List<VehicleViewModel> vehicles = new List<VehicleViewModel>()
            {
                new VehicleViewModel()
                {
                    Id = 0,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 20m, TariffPerDay = 160m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car Very fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast car"

                },
                new VehicleViewModel()
                {
                    Id = 1,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 260m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 2,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 360m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 3,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 40m, TariffPerDay = 460m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 4,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 50m, TariffPerDay = 560m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"  
                }
            };
            var vehicleViewModels = vehicles.Take(3).ToList();

            CarSharingDataViewModel model = new CarSharingDataViewModel
            {
                Vehicles = vehicleViewModels,
                NumberOfVehicles = vehicles.Count,
                NumberOfVehiclesDisplayed = 3,
                StartVehiclesIndex = 1,
                EndVehiclesIndex = vehicleViewModels.Count,
                PageNumber = 1
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult ChangeDisplayedVehiclesCount(string data)
        {
            int numberOfVehiclesToDisplay = int.Parse(data);
            //var vehicleViewModels = mapper.Map<IEnumerable<VehicleViewModel>>(vehiclesRepository.GetAllVehicles()).Take(numberOfVehiclesToDisplay).ToList();

            List<VehicleViewModel> vehicles = new List<VehicleViewModel>()
            {
                new VehicleViewModel()
                {
                    Id = 0,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 20m, TariffPerDay = 160m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car Very fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast car"

                },
                new VehicleViewModel()
                {
                    Id = 1,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 260m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 2,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 360m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 3,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 40m, TariffPerDay = 460m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 4,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 50m, TariffPerDay = 560m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                }
            };
            var vehicleViewModels = vehicles.Take(numberOfVehiclesToDisplay).ToList();

            CarSharingDataViewModel model = new CarSharingDataViewModel
            {
                Vehicles = vehicleViewModels,
                NumberOfVehicles = vehicles.Count,
                NumberOfVehiclesDisplayed = numberOfVehiclesToDisplay,
                StartVehiclesIndex = 1,
                EndVehiclesIndex = vehicleViewModels.Count,
                PageNumber = 1
            };

            return View("Index", model);
        }

        public IActionResult MoveToTheNextPage(string nextPage, string currentNumberOfVehiclesToBeDisplayed)
        {

            CarSharingDataViewModel model = new CarSharingDataViewModel();

            return View("Index", model);
        }

        public IActionResult Check(int page = 1)
        {

            const int pageSize = 6;
            if (page < 1)
                page = 1;

            List<VehicleViewModel> vehicles = new List<VehicleViewModel>()
            {
                new VehicleViewModel()
                {
                    Id = 0,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 20m, TariffPerDay = 160m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car Very fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast carVery fast car"

                },
                new VehicleViewModel()
                {
                    Id = 1,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 260m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 2,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 30m, TariffPerDay = 360m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 3,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 40m, TariffPerDay = 460m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                },
                new VehicleViewModel()
                {
                    Id = 4,
                    Name = "BMW x6",
                    Tariff = new Models.VehicleData.Includes.Tariff() {TariffPerHour = 50m, TariffPerDay = 560m},
                    Image = "../../../Images/BMW_x6.jpg",
                    BriefDescription = "Very fast car"
                }
            };

            int vehiclesCount = vehicles.Count();

            var pager = new Pager(vehiclesCount, page, pageSize);

            int vehiclesSkip = (page - 1) * pageSize;

            var vehiclesData = vehicles.Skip(vehiclesSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View();
        }
    }
}
