using AutoMapper;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Interfaces;
using CarSharingApp.Services.Includes;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class ShareYourCarController : Controller
    {

        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly IFileUploadService fileUploadService;
        private readonly ICurrentUserStatusProvider currentUserStatusProvider;


        public ShareYourCarController(IVehiclesRepository vehiclesRepository, IMapper mapper, IFileUploadService fileUploadService, ICurrentUserStatusProvider currentUserStatusProvider)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.fileUploadService = fileUploadService;
            this.currentUserStatusProvider = currentUserStatusProvider;
        }

        // Return basic view with page
        public IActionResult Index()
        {
            if (currentUserStatusProvider.GetUserRole() != UserRole.Client)
            {
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

        // When user submit his information about car to share with (model and car image selected in input tag)
        [HttpPost]
        public async Task<IActionResult> SaveSharedCar(VehicleShareModel vehicleShareModel, IFormFile file)
        {
            // If validation error occured -> return same view with errors and current model
            if (!ModelState.IsValid)
                return View("Index", vehicleShareModel);

            if (file != null)
                vehicleShareModel.Image = await fileUploadService.UploadFileAsync(file);

            var vehicleModel = mapper.Map<VehicleModel>(vehicleShareModel);

            if(currentUserStatusProvider.GetUserId() != null)
                vehicleModel.OwnerId = (int)currentUserStatusProvider.GetUserId();

            vehiclesRepository.ShareNewVehicle(vehicleModel);

            return RedirectToAction("Index", "CarSharing");
        }
    }
}
