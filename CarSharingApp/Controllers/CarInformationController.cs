using AutoMapper;
using CarSharingApp.Models.RatingData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class CarInformationController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly IClientsRepository clientsRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IOrdersRepository ordersRepository;

        public CarInformationController(IVehiclesRepository vehiclesRepository, IMapper mapper, IClientsRepository clientsRepository, IRatingRepository ratingRepository, 
                                        IOrdersRepository ordersRepository)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.clientsRepository = clientsRepository;
            this.ratingRepository = ratingRepository;
            this.ordersRepository = ordersRepository;
        }

        public IActionResult Index(int vehicleId)
        {
            VehicleModel vehicle = vehiclesRepository.GetVehicleById(vehicleId);

            var vehicleViewModel = mapper.Map<VehicleInformationViewModel>(vehicle);

            vehicleViewModel.OwnerUsername = clientsRepository.GetClientUsername(vehicle.OwnerId);
            vehicleViewModel.Rating = mapper.Map<VehicleRatingViewModel>(ratingRepository.GetVehicleRatingById(vehicle.RatingId));
            vehicleViewModel.TimesOrdered = ordersRepository.GetNumberOfVehicleOrders(vehicleId);

            return View(vehicleViewModel);
        }
    }
}
