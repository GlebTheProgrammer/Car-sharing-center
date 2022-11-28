using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class ClientProfileController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IClientsRepository clientsRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserStatusProvider userStatusProvider;

        public ClientProfileController(IVehiclesRepository vehiclesRepository, IClientsRepository clientsRepository, IMapper mapper, ICurrentUserStatusProvider userStatusProvider)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.clientsRepository = clientsRepository;
            this.mapper = mapper;
            this.userStatusProvider = userStatusProvider;
        }

        public IActionResult Index(int vehicleId)
        {
            if (userStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (userStatusProvider.GetUserRole() != UserRole.Client)
            {
                userStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            var vehicle = vehiclesRepository.GetVehicleById(vehicleId);

            var client = clientsRepository.GetClientById(vehicle.OwnerId);

            var viewModel = mapper.Map<ClientProfileViewModel>(client);
            viewModel.VehicleId = vehicleId;

            return View(viewModel);
        }
    }
}
