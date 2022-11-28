using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.OrderData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using CarSharingApp.Views.UserPersonalAccount;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class UserPersonalAccountController : Controller
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;
        private readonly IOrdersRepository ordersRepository;
        private readonly IRatingRepository ratingRepository;

        public UserPersonalAccountController(IVehiclesRepository vehiclesRepository, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider, 
                                             IOrdersRepository ordersRepository, IClientsRepository clientsRepository, IRatingRepository ratingRepository)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.currentUserStatusProvider = currentUserStatusProvider;
            this.ordersRepository = ordersRepository;
            this.clientsRepository = clientsRepository;
            this.ratingRepository = ratingRepository;
        }

        public IActionResult Index()
        {
            if (currentUserStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (currentUserStatusProvider.GetUserRole() != UserRole.Client)
            {
                currentUserStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            // Проверка заказов на просроченное время
            var vehiclesIds = ordersRepository.CheckExpiredOrdersAndGetVehiclesId().Result;
            if (vehiclesIds.Count > 0)
                vehiclesRepository.ChangeVehiclesIsOrderedState(vehiclesIds, false);

            int userId = (int)currentUserStatusProvider.GetUserId();

            // Создние объекта заказов
            List<OrderInUserAccountViewModel> activeOrders = mapper.Map<List<OrderInUserAccountViewModel>>(ordersRepository.GetActiveOrdersForAUser(userId));
            foreach (var activeOrder in activeOrders)
            {
                activeOrder.VehicleName = vehiclesRepository.GetVehicleById(activeOrder.OrderedVehicleId).Name;
            }

            List<VehicleAccountViewModel> userVehicles = mapper.Map<List<VehicleAccountViewModel>>(vehiclesRepository.GetAllUserVehicles(userId));

            UserPersonalInformationDataViewModel viewModel = new UserPersonalInformationDataViewModel()
            {
                ClientAccountViewModel = mapper.Map<ClientAccountViewModel>(clientsRepository.GetClientById(userId)),

                VehiclesAdded = vehiclesRepository.GetAllUserVehicles(userId).Count(),
                ActiveVehicles = vehiclesRepository.GetAllActiveUserVehicles(userId).Count(),
                ActiveOrdersCount = ordersRepository.GetNumberOfActiveOrdersForAUser(userId),

                ActveOrders = activeOrders,
                UserVehicles = userVehicles
            };

            return View(viewModel);
        }

        public IActionResult PublishVehicleInTheCatalog(int vehicleId)
        {
            vehiclesRepository.PublishVehicleInTheCatalog(vehicleId);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveVehicleFromTheCatalog(int vehicleId)
        {
            vehiclesRepository.RemoveVehicleFromTheCatalog(vehicleId);
            return RedirectToAction("Index");
        }

        public void DeleteVehicle(int vehicleId)
        {
            ratingRepository.DeleteVehicleRating(vehiclesRepository.GetVehicleById(vehicleId).RatingId);
            vehiclesRepository.DeleteVehicle(vehicleId);
            ordersRepository.DeleteAllVehicleOrders(vehicleId);
        }

        public void FinishOrder(int orderId)
        {
            var order = ordersRepository.GetOrderById(orderId);

            vehiclesRepository.ChangeVehicleIsOrderedState(order.OrderedVehicleId, false);
            ordersRepository.FinishOrder(orderId);
        }
    }
}
