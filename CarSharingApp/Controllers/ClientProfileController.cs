using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class ClientProfileController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IClientsRepository clientsRepository;
        private readonly IMapper mapper;

        public ClientProfileController(IVehiclesRepository vehiclesRepository, IClientsRepository clientsRepository, IMapper mapper)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.clientsRepository = clientsRepository;
            this.mapper = mapper;
        }

        public IActionResult Index(int vehicleId)
        {
            var vehicle = vehiclesRepository.GetVehicleById(vehicleId);

            var client = clientsRepository.GetClientById(vehicle.OwnerId);

            var viewModel = mapper.Map<ClientProfileViewModel>(client);
            viewModel.VehicleId = vehicleId;

            return View(viewModel);
        }
    }
}
