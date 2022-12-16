using CarSharingApp.Models.ApplicationData;
using CarSharingApp.Models.MongoView;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class CatalogController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public CatalogController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            //var vehiclesIds = _repositoryManager.OrdersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            //if (vehiclesIds.Count > 0)
            //    _repositoryManager.VehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            List<VehicleCatalogModel> vehicles_CatalogRepresentation = await _mongoDbService.GetAllPublishedAndNotOrderedVehicles_CatalogRepresentation();
            int notOrderedPublishedVehiclesCount = vehicles_CatalogRepresentation.Count();

            if (page < 1)
                page = 1;
            Pager pager = new Pager(notOrderedPublishedVehiclesCount, page, pageSize);

            int vehiclesSkip = (page - 1) * pageSize;
            List<VehicleCatalogModel> vehiclesToDisplay_CatalogRepresentation = vehicles_CatalogRepresentation.Skip(vehiclesSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;
            VehiclesCatalogDataViewModel model = new VehiclesCatalogDataViewModel()
            {
                Vehicles = vehiclesToDisplay_CatalogRepresentation,
                NumberOfVehiclesToBeDisplayed = pageSize,
                NumberOfNotOrderedVehicles = notOrderedPublishedVehiclesCount,
                IndexOfFirstVehicleInTheList = vehiclesToDisplay_CatalogRepresentation.Count == 0 ? 0 : vehiclesSkip + 1,
                IndexOfLastVehicleInTheList = vehiclesSkip + pageSize > vehiclesToDisplay_CatalogRepresentation.Count ? vehiclesToDisplay_CatalogRepresentation.Count : vehiclesSkip + pageSize,
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeDisplayedVehiclesCount(string data)
        {
            int numberOfVehiclesToDisplay = int.Parse(data);

            List<VehicleCatalogModel> vehicles_CatalogRepresentation = await _mongoDbService.GetAllPublishedAndNotOrderedVehicles_CatalogRepresentation();

            VehiclesCatalogDataViewModel model = new VehiclesCatalogDataViewModel
            {
                Vehicles = vehicles_CatalogRepresentation.Take(numberOfVehiclesToDisplay).ToList(),
                NumberOfNotOrderedVehicles = vehicles_CatalogRepresentation.Count(),
                NumberOfVehiclesToBeDisplayed = numberOfVehiclesToDisplay,
                IndexOfFirstVehicleInTheList = vehicles_CatalogRepresentation.Count == 0 ? 0 : 1,
                IndexOfLastVehicleInTheList = vehicles_CatalogRepresentation.Count
            };

            return View("Index", model);
        }
    }
}
