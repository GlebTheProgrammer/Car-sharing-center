using AutoMapper;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class ShareYourCarController : Controller
    {

        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly IFileUploadService fileUploadService;


        public ShareYourCarController(IVehiclesRepository vehiclesRepository, IMapper mapper, IFileUploadService fileUploadService)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.fileUploadService = fileUploadService;
        }

        // Return basic view with page
        public IActionResult Index()
        {   
            return View(new VehicleShareModel()
            {
                Name = "hello",
                Engine = Models.VehicleData.Includes.Engine.Thermal,
                ExteriorColor = "black",
                MaxSpeed = 100,
                FuelType = Models.VehicleData.Includes.FuelType.Gasoline,
                Description = "grwesdd wdldk dwdm dedjed ddijedidj edejdijede edijeffj fejfieieoj",
                Drivetrain = Models.VehicleData.Includes.Drivetrain.Four_wheel_drive,
                BriefDescription = "world ыволоыы ыоыоы",
                InteriorColor = "white",
                Tariff = new Models.VehicleData.Includes.Tariff { TariffPerDay = 100, TariffPerHour = 10},
                Location = new Models.VehicleData.Includes.Location { Address = "L. Bedy", Latitude = "24.55", Longitude = "26.30"},
                Transmission = Models.VehicleData.Includes.Transmission.Manual,
                VIN = "12345671234561234"
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

            vehiclesRepository.ShareNewVehicle(mapper.Map<VehicleModel>(vehicleShareModel));


            return View();
        }
    }
}
