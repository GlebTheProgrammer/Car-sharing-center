using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public sealed class CustomerPersonalAccountController : Controller
    {
        //private readonly MongoDbService _mongoDbService;

        //public CustomerPersonalAccountController(MongoDbService mongoDbService)
        //{
        //    _mongoDbService = mongoDbService;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    string customerId = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("JWToken")).Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

        //    List<RentalAccountModel> activeRentals = await _mongoDbService.GetCustomerActiveRentals_AccountRepresentation(customerId);
        //    List<VehicleAccountModel> customerVehicles = await _mongoDbService.GetCustomerVehicles_AccountRepresentation(customerId);
        //    CustomerAccountModel customerAccountInformation = await _mongoDbService.GetCustomerInformation_AccountRepresentation(customerId);

        //    CustomerAccountInfoViewModel customerAccountViewModel = new CustomerAccountInfoViewModel()
        //    {
        //        CustomerAccountInformation = customerAccountInformation,
        //        CustomerVehicles = customerVehicles,
        //        ActiveRentals = activeRentals,

        //        CustomerActiveRentalsCount = activeRentals.Count,
        //        CustomerPublishedVehiclesCount = customerVehicles.Where(vehicle => vehicle.IsPublished).Count(),
        //        CustomerVehiclesCount= customerVehicles.Count
        //    };

        //    return View(customerAccountViewModel);
        //}

        //public async Task<IActionResult> PublishVehicleInTheCatalog(string vehicleId)
        //{
        //    await _mongoDbService.PublishVehicleInTheCatalog(vehicleId);

        //    return RedirectToAction("Index");
        //}

        //public async Task<IActionResult> RemoveVehicleFromTheCatalog(string vehicleId)
        //{
        //    await _mongoDbService.HideVehicleFromTheCatalog(vehicleId);

        //    return RedirectToAction("Index");
        //}

        //public async void DeleteVehicle(string vehicleId)
        //{
        //    await _mongoDbService.DeleteVehicle(vehicleId);
        //}

        //[HttpPost]
        //public async Task<IActionResult> FinishActiveRental(string rentalId, int conditionRating, int fuelConsumptionRating, int easyToDriveRating, int familyFriendlyRating, int suvRating, bool hasSubmittedRating)
        //{
        //    Rental rentalInformation = await _mongoDbService.GetRentalInformation(rentalId);

        //    if (hasSubmittedRating)
        //    {
        //        RatingSubmitModel rentedVehicleRating = new RatingSubmitModel()
        //        {
        //            Condition = conditionRating,
        //            FuelConsumption = fuelConsumptionRating,
        //            EasyToDrive = easyToDriveRating,
        //            FamilyFriendly = familyFriendlyRating,
        //            SUV = suvRating
        //        };

        //        await _mongoDbService.SubmitCustomerRating(rentalInformation?.RentedVehicleId, rentedVehicleRating);
        //    }

        //    await _mongoDbService.FinishActiveRental(rentalId);

        //    HttpContext.Session.SetString("FinishedActiveRental", "true");

        //    return RedirectToAction("Index");
        //}
    }
}
