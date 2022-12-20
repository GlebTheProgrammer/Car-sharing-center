using CarSharingApp.Models.MongoView;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class EditVehicleDataController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public EditVehicleDataController(MongoDbService mongoDbService)
        {
            _mongoDbService= mongoDbService;
        }

        public async Task<IActionResult> Index(string vehicleId)
        {
            VehicleEditModel vehicleEditModel = await _mongoDbService.GetVehicle_EditRepresentation(vehicleId);

            return View(vehicleEditModel);
        }

        public async Task<IActionResult> EditVehicleData(VehicleEditModel vehicleEditModel)
        {
            if (!ModelState.IsValid)
                return View("Index", vehicleEditModel);

            await _mongoDbService.EditVehicleData(vehicleEditModel);

            HttpContext.Session.SetString("ChangedVehicleData", "true");

            return RedirectToAction("Index", new { vehicleId = vehicleEditModel.VehicleId });
        }
    }
}
