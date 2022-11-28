using AutoMapper;
using CarSharingApp.Models.ApplicationData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using CarSharingApp.Views.CarSharing;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CarSharingApp.Controllers
{
    public class CarSharingController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IOrdersRepository ordersRepository;


        public CarSharingController(IVehiclesRepository vehiclesRepository, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider, IHttpContextAccessor httpContextAccessor, IOrdersRepository ordersRepository)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.currentUserStatusProvider = currentUserStatusProvider;
            this.httpContextAccessor = httpContextAccessor;
            this.ordersRepository = ordersRepository;
        }



        public IActionResult Index(int page = 1, string pageSizeStr = "3")
        {
            // Проверка заказов на просроченное время
            var vehiclesIds = ordersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            if (vehiclesIds.Count > 0)
                vehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            // Получаем только те модельки, которые не относятся к текущему пользователю
            var vehicleViewModels = mapper.Map<IEnumerable<VehicleViewModel>>(vehiclesRepository.GetAllVehiclesForCatalog().Where(vehicle => vehicle.OwnerId != currentUserStatusProvider.GetUserId())).ToList();

            int pageSize = int.Parse(pageSizeStr);
            if (page < 1)
                page = 1;

            int vehiclesCount = vehicleViewModels.Count();

            var pager = new Pager(vehiclesCount, page, pageSize);

            int vehiclesSkip = (page - 1) * pageSize;

            var vehiclesData = vehicleViewModels.Skip(vehiclesSkip).Take(pager.PageSize).ToList();

            foreach (var vehicle in vehiclesData)
            {
                vehicle.LastTimeOrdered = ordersRepository.GetLastOrderExpiredDate(vehicle.Id);
            }

            this.ViewBag.Pager = pager;

            CarSharingDataViewModel model = new CarSharingDataViewModel
            {
                Vehicles = vehiclesData,
                NumberOfVehicles = vehicleViewModels.Count,
                NumberOfVehiclesDisplayed = pageSize,
                StartVehiclesIndex = vehicleViewModels.Count == 0 ? 0 : vehiclesSkip + 1,
                EndVehiclesIndex = vehiclesSkip + pageSize > vehicleViewModels.Count ? vehicleViewModels.Count : vehiclesSkip + pageSize,
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult ChangeDisplayedVehiclesCount(string data)
        {
            int numberOfVehiclesToDisplay = int.Parse(data);
            var vehicleViewModels = mapper.Map<IEnumerable<VehicleViewModel>>(vehiclesRepository.GetAllVehiclesForCatalog()).ToList();

            CarSharingDataViewModel model = new CarSharingDataViewModel
            {
                Vehicles = vehicleViewModels.Take(numberOfVehiclesToDisplay).ToList(),
                NumberOfVehicles = vehicleViewModels.Count,
                NumberOfVehiclesDisplayed = numberOfVehiclesToDisplay,
                StartVehiclesIndex = vehicleViewModels.Count == 0 ? 0 : 1,
                EndVehiclesIndex = vehicleViewModels.Count
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
