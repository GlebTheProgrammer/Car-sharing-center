using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.Domain.Enums;
using CarSharingApp.PublicApi.Primitives;
using CarSharingApp.Domain.ValueObjects;
using CarSharingApp.Domain.SmartEnums;
using static CarSharingApp.Domain.Enums.FlagEnums;
using System.Globalization;

namespace CarSharingApp.PublicApi.Controllers
{
    [Route("api/vehicles")]
    public sealed class VehiclesController : ApiController
    {
        private readonly IVehicleService _vehicleService;
        private readonly ICustomerService _customerService;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService vehicleService, 
                                  ICustomerService customerService, 
                                  ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService;
            _customerService = customerService;                             
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(Guid.Parse(jwtClaims.Id), request);

            if (requestToVehicleResult.IsError)
            {
                _logger.LogInformation("Customer with ID: {customerId} entered wrong data trying to add new vehicle.", jwtClaims.Id);
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            await _vehicleService.CreateVehicleAsync(vehicle);

            _logger.LogInformation("Customer with ID: {customerId} has successfully added new vehicle with ID: {vehicleId}.", jwtClaims.Id, vehicle.Id);

            return CreatedAtAction(
                        actionName: nameof(GetVehicle),
                        routeValues: new { id = vehicle.Id },
                        value: MapVehicleResponse(vehicle));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetVehicle([FromRoute] Guid id)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            return getVehicleResult.Match(
                vehicle => Ok(MapVehicleResponse(vehicle)),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}/information/edit")]
        [Authorize]
        public async Task<IActionResult> GetVehicleEditInformation([FromRoute] Guid id)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            return getVehicleResult.Match(
                vehicle => Ok(MapEditVehicleInformationResponse(vehicle)),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}/information")]
        [Authorize]
        public async Task<IActionResult> GetVehicleInformation([FromRoute] Guid id)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);
            if (getVehicleResult.IsError)
            {
                _logger.LogInformation("Failed finding information of the vehicle with ID: {vehicleId} for the customer with ID: {customerId}.", id, jwtClaims.Id);
                return Problem(getVehicleResult.Errors);
            }
            Vehicle vehicle = getVehicleResult.Value;

            ErrorOr<Customer> getCustomerResult = await _customerService.GetCustomerAsync(vehicle.CustomerId);
            if (getCustomerResult.IsError)
            {
                _logger.LogInformation("Failed finding information of the owner with ID: {ownerId} of the vehicle with ID: {vehicleId} for the customer with ID: {customerId}.", vehicle.CustomerId, id, jwtClaims.Id);
                return Problem(getCustomerResult.Errors);
            }
            Customer customer = getCustomerResult.Value;

            return Ok(MapVehicleInformationResponse(vehicle, customer, jwtClaims.Id));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("mapRepresentation")]
        public async Task<ActionResult> GetVehiclesMapRepresentation()
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            return Ok(MapVehicleMapResponse(getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin && !v.Status.IsOrdered).ToList()));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("catalogRepresentation")]
        public async Task<IActionResult> GetVehiclesCatalogRepresentation()
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            return Ok(MapVehicleCatalogResponse(getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin && !v.Status.IsOrdered).ToList()));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("criteriaCatalogRepresentation")]
        public async Task<IActionResult> GetVehiclesByCriteria([FromQuery] GetVehiclesByCriteriaRequest request)
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            List<Vehicle> publisedAndApprovedVehicles = getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin && !v.Status.IsOrdered).ToList();

            bool tryParseHourlyPrice = decimal.TryParse(request.MaxHourlyRentalPrice, out decimal maxHourlyPrice);
            if (!tryParseHourlyPrice || maxHourlyPrice == 0)
                maxHourlyPrice = Tariff.MaxPrice;
            bool tryParseDailyPrice = decimal.TryParse(request.MaxDailyRentalPrice, out decimal maxDailyPrice);
            if (!tryParseDailyPrice || maxDailyPrice == 0)
                maxDailyPrice = Tariff.MaxPrice;


            Country? country = request.Country == null ? null : Country.FromName(request.Country);
            Colour? exteriorColor = request.ExteriorColor == null ? null : Colour.FromName(request.ExteriorColor);
            Colour? interiorColor = request.InteriorColor == null ? null : Colour.FromName(request.InteriorColor);
            Drivetrain? drivetrain = request.Drivetrain == null ? null : Drivetrain.FromName(request.Drivetrain);
            FuelType? fuelType = request.FuelType == null ? null : FuelType.FromName(request.FuelType);
            Transmission? transmission = request.Transmission == null ? null : Transmission.FromName(request.Transmission);
            Engine? engine = request.Engine == null ? null : Engine.FromName(request.Engine);
            ErrorOr<Categories> categories = request.Categories == null ? Categories.None : GetCategoriesFromList(request.Categories.Split(',').ToList());

            List<Vehicle> vehiclesThatMatchCriteria =
                publisedAndApprovedVehicles.Where(v => (v.Tariff.HourlyRentalPrice <= maxHourlyPrice && v.Tariff.DailyRentalPrice <= maxDailyPrice) &&
                                                       (country == null ? true : v.Location.Country.Name == country.Name) &&
                                                       (exteriorColor == null ? true : v.Specifications.ExteriorColor.Name == exteriorColor.Name) &&
                                                       (interiorColor == null ? true : v.Specifications.InteriorColor.Name == interiorColor.Name) &&
                                                       (drivetrain == null ? true : v.Specifications.Drivetrain.Name == drivetrain.Name) &&
                                                       (fuelType == null ? true : v.Specifications.FuelType.Name == fuelType.Name) &&
                                                       (transmission == null ? true : v.Specifications.Transmission.Name == transmission.Name) &&
                                                       (engine == null ? true : v.Specifications.Engine.Name == engine.Name)).ToList();

            List<Vehicle> vehiclesThatMatchCriteriaAndCategories = new List<Vehicle>();
            if (!categories.IsError && categories != Categories.None)
            {
                foreach (var vehicle in vehiclesThatMatchCriteria)
                {
                    if (TwoCategoryMatch(vehicle.Categories, categories.Value))
                        vehiclesThatMatchCriteriaAndCategories.Add(vehicle);
                }
            }
            else
                vehiclesThatMatchCriteriaAndCategories = vehiclesThatMatchCriteria;

            if (request.SearchAllVehicles)
                return Ok(MapVehicleCatalogResponse(vehiclesThatMatchCriteriaAndCategories));
            else
            {
                JwtClaims? jwtClaims = GetJwtClaims();

                if (jwtClaims is null)
                {
                    _logger.LogInformation("Unauthorized user tried to get access for the filter for authorized customers only.");
                    throw new NullReferenceException(nameof(jwtClaims));
                }

                if (request.SearchAllExceptMyVehicles)
                    return Ok(MapVehicleCatalogResponse(vehiclesThatMatchCriteriaAndCategories.Where(v => v.CustomerId != Guid.Parse(jwtClaims.Id)).ToList()));
                else
                    return Ok(MapVehicleCatalogResponse(vehiclesThatMatchCriteriaAndCategories.Where(v => v.CustomerId == Guid.Parse(jwtClaims.Id)).ToList()));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("nearbyMapRepresentation")]
        public async Task<IActionResult> GetNearbyVehiclesMapRepresentation([FromQuery] GetNearbyVehiclesMapRepresentationRequest request)
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            List<Vehicle> publisedAndApprovedVehicles = getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin && !v.Status.IsOrdered).ToList();

            const int EarthRadiusKm = 6371;

            Dictionary<Guid, double> VehicleGuid_Distance = new Dictionary<Guid, double>();

            foreach (var vehicle in publisedAndApprovedVehicles)
            {
                var latitudeDiff = (Math.PI / 180) * double.Parse(vehicle.Location.Latitude, CultureInfo.InvariantCulture) - (Math.PI / 180) * double.Parse(request.UserLatitude, CultureInfo.InvariantCulture);
                var longitudeDiff = (Math.PI / 180) * double.Parse(vehicle.Location.Longitude, CultureInfo.InvariantCulture) - (Math.PI / 180) * double.Parse(request.UserLongitude, CultureInfo.InvariantCulture);

                var a = Math.Pow(Math.Sin(latitudeDiff / 2), 2) + Math.Cos(double.Parse(request.UserLatitude, CultureInfo.InvariantCulture)) * Math.Cos(double.Parse(vehicle.Location.Latitude, CultureInfo.InvariantCulture)) * Math.Pow(Math.Sin(longitudeDiff / 2), 2);
                var c = 2 * Math.Asin(Math.Sqrt(a));
                var kmDifference = c * EarthRadiusKm;

                VehicleGuid_Distance.Add(vehicle.Id, kmDifference);
            }

            List<Vehicle> sortedNearbyVehicles = new List<Vehicle>();
            List<double> kmDiffList = new List<double>(); 
            foreach (var vehicle in VehicleGuid_Distance.OrderBy(v => v.Value).Take(request.Count))
            {
                sortedNearbyVehicles.Add(publisedAndApprovedVehicles.Find(v => v.Id == vehicle.Key) ??
                    throw new Exception(nameof(vehicle)));
                kmDiffList.Add(vehicle.Value);
            }

            return Ok(MapNearbyVehicleMapResponse(sortedNearbyVehicles, kmDiffList));
        }


        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicle([FromRoute] Guid id, 
                                                       [FromBody] UpdateVehicleRequest request)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            if (getVehicleResult.IsError)
            {
                _logger.LogInformation("Failed finding information of the vehicle with ID: {vehicleId} when tried to change its data.", id);
                return Problem(getVehicleResult.Errors);
            }

            Vehicle notUpdatedVehicle = getVehicleResult.Value;

            if (!IsRequestAllowed(notUpdatedVehicle.CustomerId))
            {
                _logger.LogInformation("Update vehicle with  ID: {vehicleId} status request is not allowed because of missing permissions.", id);
                return Forbid();
            }

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(notUpdatedVehicle, request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            await _vehicleService.UpdateVehicleAsync(vehicle);

            _logger.LogInformation("Customer with ID: {customerId} has successfully updated vehicle with ID: {vehicleId} information.", vehicle.CustomerId, vehicle.Id);

            return NoContent();
        }

        [HttpPut("{id:guid}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicleStatus([FromRoute] Guid id, 
                                                             [FromBody] UpdateVehicleStatusRequest request)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            if (getVehicleResult.IsError)
            {
                _logger.LogInformation("Failed finding vehicle with ID: {vehicleId} when tried to change its status.", id);
                return Problem(getVehicleResult.Errors);
            }

            Vehicle notUpdatedVehicle = getVehicleResult.Value;

            if (!IsRequestAllowed(notUpdatedVehicle.CustomerId))
            {
                _logger.LogInformation("Update vehicle with ID: {vehicleId} status request is not allowed because of missing permissions.", id);
                return Forbid();
            }

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(notUpdatedVehicle, request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            await _vehicleService.UpdateVehicleStatusAsync(vehicle);

            _logger.LogInformation("Customer with ID: {customerId} has successfully updated vehicle with ID: {vehicleId} status.", vehicle.CustomerId, vehicle.Id);

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            if (getVehicleResult.IsError)
            {
                _logger.LogInformation("Failed finding vehicle with ID: {vehicleId} when tried to delete it.", id);
                return Problem(getVehicleResult.Errors);
            }

            Vehicle notDeletedVehicleYet = getVehicleResult.Value;

            if (!IsRequestAllowed(notDeletedVehicleYet.CustomerId))
            {
                _logger.LogInformation("Delete vehicle with ID: {vehicleId} request is not allowed because of missing permissions.", id);
                return Forbid();
            }

            await _vehicleService.DeleteVehicleAsync(id);

            _logger.LogInformation("Customer with ID: {customerId} has successfully deleted vehicle with ID: {vehicleId}.", notDeletedVehicleYet.CustomerId, notDeletedVehicleYet.Id);

            return NoContent();
        }

        #region Response mapping section

        [NonAction]
        private VehicleResponse MapVehicleResponse(Vehicle vehicle)
        {
            return new VehicleResponse(
                vehicle.Id,
                vehicle.CustomerId,
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                vehicle.Tariff,
                vehicle.Location,
                vehicle.Specifications,
                FlagEnums.GetListFromCategories(vehicle.Categories),
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered,
                vehicle.Status);
        }

        [NonAction]
        private VehicleInformationResponse MapVehicleInformationResponse(Vehicle vehicle, Customer owner, string requestedCustomerId)
        {
            return new VehicleInformationResponse(
                vehicle.Id.ToString(),
                owner.Credentials.Login,
                owner.Id.ToString(),
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                $"{vehicle.Tariff.HourlyRentalPrice}",
                $"{vehicle.Tariff.DailyRentalPrice}",
                vehicle.Location.StreetAddress,
                vehicle.Location.AptSuiteEtc,
                vehicle.Location.City,
                vehicle.Location.Country.Name,
                vehicle.Location.Latitude,
                vehicle.Location.Longitude,
                vehicle.Specifications.ProductionYear,
                vehicle.Specifications.MaxSpeedKph,
                vehicle.Specifications.ExteriorColor.Name,
                vehicle.Specifications.InteriorColor.Name,
                vehicle.Specifications.Drivetrain.Name,
                vehicle.Specifications.FuelType.Name,
                vehicle.Specifications.Transmission.Name,
                vehicle.Specifications.Engine.Name,
                vehicle.Specifications.VIN,
                FlagEnums.GetListFromCategories(vehicle.Categories),
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered,
                vehicle.CustomerId.ToString().Equals(requestedCustomerId));
        }

        [NonAction]
        private EditVehicleInformationResponse MapEditVehicleInformationResponse(Vehicle vehicle)
        {
            return new EditVehicleInformationResponse(
                vehicle.Id.ToString(),
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                $"{vehicle.Tariff.HourlyRentalPrice}",
                $"{vehicle.Tariff.DailyRentalPrice}",
                vehicle.Location.StreetAddress,
                vehicle.Location.AptSuiteEtc,
                vehicle.Location.City,
                vehicle.Location.Country.Name,
                vehicle.Location.Latitude,
                vehicle.Location.Longitude,
                vehicle.Specifications.ProductionYear,
                vehicle.Specifications.MaxSpeedKph,
                vehicle.Specifications.ExteriorColor.Name,
                vehicle.Specifications.InteriorColor.Name,
                vehicle.Specifications.Drivetrain.Name,
                vehicle.Specifications.FuelType.Name,
                vehicle.Specifications.Transmission.Name,
                vehicle.Specifications.Engine.Name,
                vehicle.Specifications.VIN);
        }

        [NonAction]
        private VehiclesDisplayOnMapResponse MapVehicleMapResponse(List<Vehicle> vehicles)
        {
            var resultList = new List<VehicleDisplayOnMap>();

            foreach (var vehicle in vehicles)
            {
                resultList.Add(new VehicleDisplayOnMap(
                    vehicle.Name,
                    vehicle.Image,
                    vehicle.Location.Latitude,
                    vehicle.Location.Longitude));
            }

            return new VehiclesDisplayOnMapResponse(resultList);
        }

        [NonAction]
        private NearbyVehiclesDisplayOnMapResponse MapNearbyVehicleMapResponse(List<Vehicle> vehicles, List<double> kmDiffList)
        {
            var resultList = new List<NearbyVehicleDisplayOnMapResponse>();

            int counter = 0;
            foreach (var vehicle in vehicles)
            {
                string vehicleHourlPyrice = $"{vehicle.Tariff.HourlyRentalPrice}";
                string vehicleDailyPrice = $"{vehicle.Tariff.DailyRentalPrice}";

                resultList.Add(new NearbyVehicleDisplayOnMapResponse(
                    vehicle.Name,
                    vehicle.Image,
                    vehicle.Location.Latitude,
                    vehicle.Location.Longitude,
                    vehicleHourlPyrice.Length > 3 ? vehicleHourlPyrice.Insert(vehicleHourlPyrice.Length - 3, ",") : vehicleHourlPyrice,
                    vehicleDailyPrice.Length > 3 ? vehicleDailyPrice.Insert(vehicleDailyPrice.Length - 3, ",") : vehicleDailyPrice,
                    vehicle.TimesOrdered,
                    $"{kmDiffList[counter++]}"[..4]));
            }

            return new NearbyVehiclesDisplayOnMapResponse(resultList);
        }

        [NonAction]
        private VehiclesDisplayInCatalogResponse MapVehicleCatalogResponse(List<Vehicle> vehicles)
        {
            var resultList = new List<VehicleDisplayInCatalog>();

            foreach (var vehicle in vehicles)
            {
                resultList.Add(new VehicleDisplayInCatalog(
                    vehicle.Id.ToString(),
                    vehicle.Name,
                    vehicle.Image,
                    vehicle.BriefDescription,
                    $"{vehicle.Tariff.HourlyRentalPrice}",
                    $"{vehicle.Tariff.DailyRentalPrice}",
                    vehicle.TimesOrdered));
            }

            return new VehiclesDisplayInCatalogResponse(resultList);
        }

        #endregion

        #region Controller helper methods section

        [NonAction]
        private bool IsRequestAllowed(Guid requestedId)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims == null || (jwtClaims.Id != requestedId.ToString() && jwtClaims.Role != "Administrator"))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
