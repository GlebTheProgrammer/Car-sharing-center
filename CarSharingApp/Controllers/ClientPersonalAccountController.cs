using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.OrderData;
using CarSharingApp.Models.RatingData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using CarSharingApp.Views.UserPersonalAccount;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class ClientPersonalAccountController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _currentUserStatusProvider;

        public ClientPersonalAccountController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _currentUserStatusProvider = currentUserStatusProvider;
        }

        public IActionResult Index()
        {
            if (_currentUserStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (_currentUserStatusProvider.GetUserRole() != UserRole.Client)
            {
                _currentUserStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            var vehiclesIds = _repositoryManager.OrdersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            if (vehiclesIds.Count > 0)
                _repositoryManager.VehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            int userId = (int)_currentUserStatusProvider.GetUserId();

            List<OrderInUserAccountViewModel> activeOrders = _mapper.Map<List<OrderInUserAccountViewModel>>(_repositoryManager.OrdersRepository.GetActiveOrdersForAUser(userId));
            foreach (var activeOrder in activeOrders)
            {
                activeOrder.VehicleName = _repositoryManager.VehiclesRepository.GetVehicleById(activeOrder.OrderedVehicleId).Name;
            }

            List<VehicleAccountViewModel> userVehicles = _mapper.Map<List<VehicleAccountViewModel>>(_repositoryManager.VehiclesRepository.GetAllUserVehicles(userId));

            ClientPersonalInformationDataViewModel viewModel = new ClientPersonalInformationDataViewModel()
            {
                ClientAccountViewModel = _mapper.Map<ClientAccountViewModel>(_repositoryManager.ClientsRepository.GetClientById(userId)),

                VehiclesAdded = _repositoryManager.VehiclesRepository.GetAllUserVehicles(userId).Count(),
                ActiveVehicles = _repositoryManager.VehiclesRepository.GetAllActiveUserVehicles(userId).Count(),
                ActiveOrdersCount = _repositoryManager.OrdersRepository.GetNumberOfActiveOrdersForAUser(userId),

                ActveOrders = activeOrders,
                ClientVehicles = userVehicles
            };

            return View(viewModel);
        }

        public IActionResult PublishVehicleInTheCatalog(int vehicleId)
        {
            _repositoryManager.VehiclesRepository.PublishVehicleInTheCatalog(vehicleId);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveVehicleFromTheCatalog(int vehicleId)
        {
            _repositoryManager.VehiclesRepository.RemoveVehicleFromTheCatalog(vehicleId);
            return RedirectToAction("Index");
        }

        public void DeleteVehicle(int vehicleId)
        {
            _repositoryManager.RatingRepository.DeleteVehicleRating(_repositoryManager.VehiclesRepository.GetVehicleById(vehicleId).RatingId);
            _repositoryManager.VehiclesRepository.DeleteVehicle(vehicleId);
            _repositoryManager.OrdersRepository.DeleteAllVehicleOrders(vehicleId);
        }

        [HttpPost]
        public IActionResult FinishOrder(int orderId, int conditionRating, int fuelConsumptionRating, int easyToDriveRating, int familyFriendlyRating, int suvRating, bool hasSubmittedRating)
        {
            var order = _repositoryManager.OrdersRepository.GetOrderById(orderId);

            if (hasSubmittedRating)
            {
                var vehicle = _repositoryManager.VehiclesRepository.GetVehicleById(order.OrderedVehicleId);

                ProvideRatingViewModel userRating = new ProvideRatingViewModel()
                {
                    Condition = conditionRating,
                    FuelConsumption = fuelConsumptionRating,
                    EasyToDrive = easyToDriveRating,
                    FamilyFriendly = familyFriendlyRating,
                    SUV = suvRating
                };

                _repositoryManager.RatingRepository.UpdateVehicleRating(vehicle.RatingId, userRating);
            }

            _repositoryManager.VehiclesRepository.ChangeVehicleIsOrderedState(order.OrderedVehicleId, false);
            _repositoryManager.OrdersRepository.FinishOrder(orderId);

            _currentUserStatusProvider.ChangeFinishedActiveOrderState(true);

            return RedirectToAction("Index");
        }
    }
}
