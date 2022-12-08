using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Repository.Interfaces.Includes;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class EditClientPersonalInfoController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _currentUserStatusProvider;

        public EditClientPersonalInfoController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider)
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

            var viewModel = _mapper.Map<ClientEditInfoViewModel>(_repositoryManager.ClientsRepository.GetClientById((int)_currentUserStatusProvider.GetUserId()));

            return View(viewModel);
        }

        public IActionResult ChangeClientPersonalInfo(ClientEditInfoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            var clientModel = _repositoryManager.ClientsRepository.GetClientById(viewModel.Id);

            Merge<ClientEditInfoViewModel>(clientModel, viewModel);

            _repositoryManager.ClientsRepository.UpdateClient(clientModel);

            _currentUserStatusProvider.ChangeAccountDataHasChangedState(true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeClientPassword(string newPassword)
        {

            _repositoryManager.ClientsRepository.UpdateClientPassword((int)_currentUserStatusProvider.GetUserId(), newPassword);

            _currentUserStatusProvider.ChangePasswordDataHasChangedState(true);

            return RedirectToAction("Index");
        }





        public void Merge<T>(ClientModel mainObject, T mergeObject)
        {
            Type tMainObj = typeof(ClientModel);
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
