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
    public sealed class VehiclesController : ApiController
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVehicle(CreateVehicleRequest request)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims is null)
            {
                return Forbid();
            }

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(Guid.Parse(jwtClaims.Id), request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            await _vehicleService.CreateVehicleAsync(vehicle);

            return CreatedAtAction(
                        actionName: nameof(GetVehicle),
                        routeValues: new { id = vehicle.Id },
                        value: MapVehicleResponse(vehicle));
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetVehicle(Guid id)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            return getVehicleResult.Match(
                vehicle => Ok(MapVehicleResponse(vehicle)),
                errors => Problem(errors));
        }

        [HttpGet("MapRepresentation")]
        public async Task<IActionResult> GetVehiclesMapRepresentation()
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            return Ok(MapVehicleMapResponse(getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin).ToList()));
        }

        [HttpGet("CatalogRepresentation")]
        public async Task<IActionResult> GetVehiclesCatalogRepresentation()
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            return Ok(MapVehicleCatalogResponse(getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin).ToList()));
        }

        [HttpPost("CriteriaCatalogRepresentation")]
        public async Task<IActionResult> GetVehiclesByCriteria(GetVehiclesByCriteriaRequest request)
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            List<Vehicle> publisedAndApprovedVehicles = getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin).ToList();

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
                    throw new NullReferenceException(nameof(jwtClaims));

                if (request.SearchAllExceptMyVehicles)
                    return Ok(MapVehicleCatalogResponse(vehiclesThatMatchCriteriaAndCategories.Where(v => v.CustomerId != Guid.Parse(jwtClaims.Id)).ToList()));
                else
                    return Ok(MapVehicleCatalogResponse(vehiclesThatMatchCriteriaAndCategories.Where(v => v.CustomerId == Guid.Parse(jwtClaims.Id)).ToList()));
            }
        }

        [HttpPost("NearbyVehiclesMapRepresentation")]
        public async Task<IActionResult> GetNearbyVehiclesMapRepresentation(GetNearbyVehiclesMapRepresentationRequest request)
        {
            List<Vehicle> getVehiclesResult = await _vehicleService.GetAllAsync();

            List<Vehicle> publisedAndApprovedVehicles = getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin).ToList();

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
        public async Task<IActionResult> UpdateVehicleInfo(Guid id, UpdateVehicleInfoRequest request)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            if (getVehicleResult.IsError)
            {
                return Problem(getVehicleResult.Errors);
            }

            Vehicle notUpdatedVehicle = getVehicleResult.Value;

            if (!IsRequestAllowed(notUpdatedVehicle.CustomerId))
            {
                return Forbid();
            }

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(notUpdatedVehicle, request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            await _vehicleService.UpdateVehicleAsync(vehicle);

            return NoContent();
        }

        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicleStatus(UpdateVehicleStatusRequest request)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(Guid.Parse(request.vehicleId));

            if (getVehicleResult.IsError)
            {
                return Problem(getVehicleResult.Errors);
            }

            Vehicle notUpdatedVehicle = getVehicleResult.Value;

            if (!IsRequestAllowed(notUpdatedVehicle.CustomerId))
            {
                return Forbid();
            }

            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(notUpdatedVehicle, request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            await _vehicleService.UpdateVehicleStatusAsync(vehicle);

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            if (getVehicleResult.IsError)
            {
                return Problem(getVehicleResult.Errors);
            }

            Vehicle notDeletedVehicleYet = getVehicleResult.Value;

            if (!IsRequestAllowed(notDeletedVehicleYet.CustomerId))
            {
                return Forbid();
            }

            await _vehicleService.DeleteVehicleAsync(id);

            return NoContent();
        }

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

        private bool IsRequestAllowed(Guid requestedId)
        {
            JwtClaims? jwtClaims = GetJwtClaims();

            if (jwtClaims == null || (jwtClaims.Id != requestedId.ToString() && jwtClaims.Role != "Administrator"))
            {
                return false;
            }

            return true;
        }
    }
}
