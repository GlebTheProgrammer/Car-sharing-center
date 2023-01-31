using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using CarSharingApp.Infrastructure.Authentication;
using CarSharingApp.Domain.Enums;
using CarSharingApp.PublicApi.Primitives;

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

            return Ok(MapVehicleResponse(getVehiclesResult.Where(v => v.Status.IsPublished && v.Status.IsConfirmedByAdmin).ToList()));
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

        private VehiclesDisplayOnMapResponse MapVehicleResponse(List<Vehicle> vehicles)
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
