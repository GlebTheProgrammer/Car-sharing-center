using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarSharingApp.Repository.MongoDbRepository;
using CarSharingApp.Models.MongoView;
using Microsoft.AspNetCore.Authorization;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public class AddVehicleController : Controller
    {
        private readonly MongoDbService _mongoDbService;
        private readonly IFileUploadService _fileUploadService;

        public AddVehicleController(MongoDbService mongoDbService, IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
            _mongoDbService = mongoDbService;
        }

        public IActionResult Index()
        {
            var emptyVehicleModel = new VehicleCreateModel();

            return View(emptyVehicleModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddVehicle(VehicleCreateModel newVehicle, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View("Index", newVehicle);

            if (file != null)
                newVehicle.Image = await _fileUploadService.UploadFileAsync(file);
            else
                throw new Exception("Image can't be null");

            await _mongoDbService.AddNewVehicleAsync(newVehicle, HttpContext);

            HttpContext.Session.SetString("AddedNewVehicle", "true");

            return RedirectToAction("Index", "Catalog");
        }
    }
}
