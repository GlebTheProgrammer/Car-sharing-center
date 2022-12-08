using AutoMapper;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Services.Interfaces;
using CarSharingApp.Services.Includes;
using Microsoft.AspNetCore.Mvc;
using CarSharingApp.Repository.Interfaces;

namespace CarSharingApp.Controllers
{
    public class AddVehicleController : Controller
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICurrentUserStatusProvider _currentUserStatusProvider;


        public AddVehicleController(IRepositoryManager repositoryManager, IMapper mapper, IFileUploadService fileUploadService, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _fileUploadService = fileUploadService;
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

            return View(new VehicleShareModel()
            {
                Location = new Models.VehicleData.Includes.Location
                {
                    Latitude = null,
                    Longitude = null
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle(VehicleShareModel vehicleShareModel, IFormFile file)
        {
            if (vehicleShareModel.Location.Latitude != null && vehicleShareModel.Location.Latitude.Any(x => char.IsLetter(x)))
            {
                vehicleShareModel.Location = new Models.VehicleData.Includes.Location
                {
                    Latitude = null,
                    Longitude = null
                };
            }

            if (!ModelState.IsValid)
            {
                return View("Index", vehicleShareModel);
            }    

            if (file != null)
                vehicleShareModel.Image = await _fileUploadService.UploadFileAsync(file);

            var vehicleModel = _mapper.Map<VehicleModel>(vehicleShareModel);

            if(_currentUserStatusProvider.GetUserId() != null)
                vehicleModel.OwnerId = (int)_currentUserStatusProvider.GetUserId();

            vehicleModel.RatingId = _repositoryManager.RatingRepository.CreateNewVehicleRating().Result;
            vehicleModel.PublishedTime = DateTime.Now;
            vehicleModel.IsPublished = false;
            vehicleModel.IsOrdered = false;
            vehicleModel.TimesOrdered = 0;

            _repositoryManager.VehiclesRepository.ShareNewVehicle(vehicleModel);

            _currentUserStatusProvider.ChangeSharedNewVehicleState(true);

            return RedirectToAction("Index", "Catalog");
        }
    }
}
