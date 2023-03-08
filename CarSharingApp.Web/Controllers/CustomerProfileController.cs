using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public sealed class CustomerProfileController : Controller
    {
        //private readonly MongoDbService _mongoDbService;

        //public CustomerProfileController(MongoDbService mongoDbService)
        //{
        //    _mongoDbService = mongoDbService;
        //}

        //public async Task<IActionResult> Index(string vehicleId)
        //{

        //    Vehicle vehicle = await _mongoDbService.GetVehicleById(vehicleId);
        //    Customer vehicleOwner = await _mongoDbService.GetCustomerById(vehicle.OwnerId);

        //    CustomerProfileModel customerProfile = await _mongoDbService.GetCustomerProfile(vehicleOwner.Id, vehicleId);

        //    return View(customerProfile);
        //}
    }
}
