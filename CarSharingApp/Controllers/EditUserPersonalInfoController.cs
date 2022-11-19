using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class EditUserPersonalInfoController : Controller
    {
        private readonly IClientsRepository clientsRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;

        public EditUserPersonalInfoController(IClientsRepository clientsRepository, IMapper mapper, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            this.clientsRepository = clientsRepository;
            this.mapper = mapper;
            this.currentUserStatusProvider = currentUserStatusProvider;
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

            var viewModel = mapper.Map<ClientEditInfoViewModel>(clientsRepository.GetClientById((int)currentUserStatusProvider.GetUserId()));

            return View(viewModel);
        }

        public IActionResult ChangeUserPersonalInfo(ClientEditInfoViewModel viewModel)
        {
            // If validation error occured -> return same view with errors and current model
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            var userModel = clientsRepository.GetClientById(viewModel.Id);

            Merge<ClientEditInfoViewModel>(userModel, viewModel);

            clientsRepository.UpdateClient(userModel);

            currentUserStatusProvider.ChangeAccountDataHasChangedState(true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeUserPassword(string newPassword)
        {

            clientsRepository.UpdateClientPassword((int)currentUserStatusProvider.GetUserId(), newPassword);

            currentUserStatusProvider.ChangePasswordDataHasChangedState(true);

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
