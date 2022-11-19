using AutoMapper;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class EditVehicleInfoController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;

        public EditVehicleInfoController(IVehiclesRepository vehiclesRepository, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.currentUserStatusProvider = currentUserStatusProvider;
        }

        public IActionResult Index(int vehicleId)
        {
            var viewModel = mapper.Map<VehicleEditInfoViewModel>(vehiclesRepository.GetVehicleById(vehicleId));

            return View(viewModel);
        }
    }
}
