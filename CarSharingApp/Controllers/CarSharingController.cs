using AutoMapper;
using CarSharingApp.Models.ApplicationData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Interfaces;
using CarSharingApp.Views.CarSharing;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class CarSharingController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _currentUserStatusProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CarSharingController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _currentUserStatusProvider = currentUserStatusProvider;
            _httpContextAccessor = httpContextAccessor;
        }



        public IActionResult Index(int page = 1, int pageSize = 3)
        {
            var vehiclesIds = _repositoryManager.OrdersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            if (vehiclesIds.Count > 0)
                _repositoryManager.VehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            var vehicleViewModels = _mapper.Map<IEnumerable<VehicleViewModel>>(_repositoryManager.VehiclesRepository.GetAllVehiclesForCatalog().Where(vehicle => vehicle.OwnerId != _currentUserStatusProvider.GetUserId())).ToList();

            if (page < 1)
                page = 1;

            int vehiclesCount = vehicleViewModels.Count();

            var pager = new Pager(vehiclesCount, page, pageSize);

            int vehiclesSkip = (page - 1) * pageSize;

            var vehiclesData = vehicleViewModels.Skip(vehiclesSkip).Take(pager.PageSize).ToList();

            foreach (var vehicle in vehiclesData)
            {
                vehicle.LastTimeOrdered = _repositoryManager.OrdersRepository.GetLastOrderExpiredDate(vehicle.Id);
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
            var vehicleViewModels = _mapper.Map<IEnumerable<VehicleViewModel>>(_repositoryManager.VehiclesRepository.GetAllVehiclesForCatalog()).ToList();

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
    }
}
