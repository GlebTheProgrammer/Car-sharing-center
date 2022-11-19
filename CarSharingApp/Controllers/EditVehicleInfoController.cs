using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
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
            if (currentUserStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (currentUserStatusProvider.GetUserRole() != UserRole.Client)
            {
                currentUserStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            var viewModel = mapper.Map<VehicleEditInfoViewModel>(vehiclesRepository.GetVehicleById(vehicleId));

            return View(viewModel);
        }

        public IActionResult UpdateVehicle(VehicleEditInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", viewModel);

            var vehicleModel = vehiclesRepository.GetVehicleById(viewModel.Id);

            Merge<VehicleEditInfoViewModel>(vehicleModel, viewModel);

            vehiclesRepository.UpdateVehicle(vehicleModel);

            currentUserStatusProvider.ChangeVehicleDataHasChangedState(true);

            return RedirectToAction("Index");
        }


        public void Merge<T>(VehicleModel mainObject, T mergeObject)
        {
            Type tMainObj = typeof(VehicleModel);
            Type tMergeObject = typeof(T);

            var mainObjProperties = tMainObj.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);
            var mergeObjProperties = tMergeObject.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in mergeObjProperties)
            {
                if (tMainObj.GetProperty(prop.Name) != null)
                {
                    var value = prop.GetValue(mergeObject, null);
                    if (value != null)
                        tMainObj.GetProperty(prop.Name)?.SetValue(mainObject, value, null);
                }
            }
        }
    }
}
