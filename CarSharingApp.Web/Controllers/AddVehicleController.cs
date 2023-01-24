using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Application.Contracts.Vehicle;
using System.Net;
using CarSharingApp.Application.Contracts.ErrorType;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace CarSharingApp.Controllers
{
    public sealed class AddVehicleController : Controller
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        public AddVehicleController(IVehicleServicePublicApiClient vehicleServiceClient, IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
            _vehicleServiceClient = vehicleServiceClient;
        }

        [Authorize]
        public IActionResult Index()
        {
            var createNewVehicleRequest = ConfigureNewCreateVehicleRequest();

            return View(createNewVehicleRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddVehicle(CreateVehicleRequest createVehicleRequest, IFormFile file)
        {
            string vehicleGeneratedImageName = string.Empty;
            if (file is not null)
                vehicleGeneratedImageName = await _fileUploadService.UploadFileAsync(file);
            else
                throw new Exception("Image can't be null");

            var response = await _vehicleServiceClient.CreateNewVehicle(ConfigureNewCreateVehicleRequest(createVehicleRequest, vehicleGeneratedImageName));

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

                        _fileUploadService.UnloadFile(vehicleGeneratedImageName);

                        return View("Index", createVehicleRequest);
                    }

                default:
                    break;
            }

            HttpContext.Session.SetString("AddedNewVehicle", "true");

            return RedirectToAction("Index", "Catalog");
        }

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
    }
}
