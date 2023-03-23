using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [Authorize]
    [Route("vehicle/information/edit")]
    public sealed class EditVehicleDataController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public EditVehicleDataController(IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] Guid vehicleId)
        {
            var response = await _vehicleServiceClient.GetVehicleInformationForEdit(vehicleId);

            response.EnsureSuccessStatusCode();

            EditVehicleInformationResponse responseModel = await response.Content.ReadFromJsonAsync<EditVehicleInformationResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return View(responseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVehicleInformationResponse editVehicle)
        {
            if (!ModelState.IsValid)
                return View("Index", editVehicle);

            var response = await _vehicleServiceClient.UpdateVehicleInformation(
                Guid.Parse(editVehicle.Id),
                MapUpdateVehicleRequest(editVehicle));

            response.EnsureSuccessStatusCode();

            HttpContext.Session.SetString("ChangedVehicleData", "true");

            return RedirectToAction("Index", new { vehicleId = editVehicle.Id });
        }

        private UpdateVehicleRequest MapUpdateVehicleRequest(EditVehicleInformationResponse editVehicle)
        {
            return new UpdateVehicleRequest(
                editVehicle.BriefDescription,
                editVehicle.Description,
                decimal.Parse(editVehicle.HourlyRentalPrice, System.Globalization.CultureInfo.InvariantCulture),
                decimal.Parse(editVehicle.DailyRentalPrice, System.Globalization.CultureInfo.InvariantCulture),
                editVehicle.StreetAddress,
                editVehicle.AptSuiteEtc,
                editVehicle.City,
                editVehicle.Country,
                editVehicle.Latitude,
                editVehicle.Longitude);
        }
    }
}
