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
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _userStatusProvider;

        public ClientProfileController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider userStatusProvider)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _userStatusProvider = userStatusProvider;
        }

        public IActionResult Index(int vehicleId)
        {
            if (_userStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (_userStatusProvider.GetUserRole() != UserRole.Client)
            {
                _userStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            var vehicle = _repositoryManager.VehiclesRepository.GetVehicleById(vehicleId);

            var client = _repositoryManager.ClientsRepository.GetClientById(vehicle.OwnerId);

            var viewModel = _mapper.Map<ClientProfileViewModel>(client);
            viewModel.VehicleId = vehicleId;

            return View(viewModel);
        }
    }
}
