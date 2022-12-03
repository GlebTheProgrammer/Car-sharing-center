using AutoMapper;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class EditVehicleInfoController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _currentUserStatusProvider;

        public EditVehicleInfoController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            _repositoryManager= repositoryManager;
            _mapper= mapper;
            _currentUserStatusProvider= currentUserStatusProvider;
        }

        public IActionResult Index(int vehicleId)
        {
            if (_currentUserStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (_currentUserStatusProvider.GetUserRole() != UserRole.Client)
            {
                _currentUserStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            var viewModel = _mapper.Map<VehicleEditInfoViewModel>(_repositoryManager.VehiclesRepository.GetVehicleById(vehicleId));

            return View(viewModel);
        }

        public IActionResult UpdateVehicle(VehicleEditInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", viewModel);

            var vehicleModel = _repositoryManager.VehiclesRepository.GetVehicleById(viewModel.Id);

            Merge<VehicleEditInfoViewModel>(vehicleModel, viewModel);

            _repositoryManager.VehiclesRepository.UpdateVehicle(vehicleModel);

            _currentUserStatusProvider.ChangeVehicleDataHasChangedState(true);

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
