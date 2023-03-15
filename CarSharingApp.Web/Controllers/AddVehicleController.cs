using Microsoft.AspNetCore.Mvc;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Application.Contracts.Vehicle;
using System.Net;
using CarSharingApp.Application.Contracts.ErrorType;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using CarSharingApp.Web.Extensions;

namespace CarSharingApp.Controllers
{
    [Authorize]
    [Route("vehicle/share")]
    public sealed class AddVehicleController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;
        private readonly IAzureBlobStoragePublicApiClient _blobStorageClient;

        public AddVehicleController(IVehicleServicePublicApiClient vehicleServiceClient, 
                                    IAzureBlobStoragePublicApiClient blobStorageClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
            _blobStorageClient = blobStorageClient;
        }

        #region AddVehicle View action methods section

        [HttpGet]
        public IActionResult Index()
        {
            var createNewVehicleRequest = ConfigureNewCreateVehicleRequest();

            return View(createNewVehicleRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public async Task<IActionResult> AddVehicle([FromForm] CreateVehicleRequest createVehicleRequest, 
                                                    [FromForm] IFormFile file)
        {
            if (file is null)
                throw new Exception("Image can't be null");

            DateTime dateTime = DateTime.UtcNow;
            string imageName = $"{dateTime.Hour}_{dateTime.Minute}_{dateTime.Second}_{file.FileName}";

            var response = await _vehicleServiceClient.CreateNewVehicle(ConfigureNewCreateVehicleRequest(createVehicleRequest, imageName));

            string responseContent = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return RedirectToAction("Unauthorized401Error", "CustomExceptionHandle");

                case HttpStatusCode.BadRequest:
                    {
                        ValidationError validationError = JsonSerializer.Deserialize<ValidationError>(responseContent) ?? new ValidationError();

                        foreach (var error in validationError.Errors)
                        {
                            ModelState.AddModelError(error.Key.Contains('.') ? error.Key.Substring(error.Key.LastIndexOf('.')) : error.Key,
                                error.Value.FirstOrDefault() ?? string.Empty);
                        }

                        return View("Index", createVehicleRequest);
                    }

                default:
                    break;
            }

            await _blobStorageClient.UploadIFormFileBlobAsync(file, imageName);

            HttpContext.Session.SetString("AddedNewVehicle", "true");

            return RedirectToAction("Index", "Catalog");
        }

        #endregion

        #region Requests configuration section

        [NonAction]
        private CreateVehicleRequest ConfigureNewCreateVehicleRequest(CreateVehicleRequest? requestFromView = null, string? image = null)
        {
            return new CreateVehicleRequest(
                Name: requestFromView?.Name ?? string.Empty,
                Image: image ?? string.Empty,
                BriefDescription: requestFromView?.BriefDescription ?? string.Empty,
                Description: requestFromView?.Description ?? string.Empty,
                HourlyRentalPrice: requestFromView?.HourlyRentalPrice ?? default,
                DailyRentalPrice: requestFromView?.DailyRentalPrice ?? default,
                StreetAddress: requestFromView?.StreetAddress ?? string.Empty,
                AptSuiteEtc: requestFromView?.AptSuiteEtc ?? string.Empty,
                City: requestFromView?.City ?? string.Empty,
                Country: requestFromView?.Country ?? string.Empty,
                Latitude: requestFromView?.Latitude ?? string.Empty,
                Longitude: requestFromView?.Longitude ?? string.Empty,
                ProductionYear: requestFromView?.ProductionYear ?? default,
                MaxSpeedKph: requestFromView?.MaxSpeedKph ?? default,
                ExteriorColor: requestFromView?.ExteriorColor ?? string.Empty,
                InteriorColor: requestFromView?.InteriorColor ?? string.Empty,
                Drivetrain: requestFromView?.Drivetrain ?? string.Empty,
                FuelType: requestFromView?.FuelType ?? string.Empty,
                Transmission: requestFromView?.Transmission ?? string.Empty,
                Engine: requestFromView?.Engine ?? string.Empty,
                VIN: requestFromView?.VIN ?? string.Empty,
                Categories: requestFromView?.Categories ?? string.Empty);
        }

        #endregion
    }
}
