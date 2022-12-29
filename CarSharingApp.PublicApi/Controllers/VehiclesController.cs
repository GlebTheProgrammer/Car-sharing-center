using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;

namespace CarSharingApp.PublicApi.Controllers
{
    [Authorize]
    public class VehiclesController : ApiController
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("{customerId:guid}")]
        public async Task<IActionResult> CreateVehicle(Guid customerId, CreateVehicleRequest request)
        {
            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(customerId, request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            ErrorOr<Created> createVehicleResult =  await _vehicleService.CreateVehicleAsync(vehicle);

            return createVehicleResult.Match(
                created => CreatedAtAction(
                        actionName: nameof(GetVehicle),
                        routeValues: new { id = vehicle.Id },
                        value: MapVehicleResponse(vehicle)),
                errors => Problem(errors));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVehicle(Guid id)
        {
            ErrorOr<Vehicle> getVehicleResult = await _vehicleService.GetVehicleAsync(id);

            return getVehicleResult.Match(
                vehicle => Ok(MapVehicleResponse(vehicle)),
                errors => Problem(errors));

        }

        [HttpPut("{customerId:guid}/{id:guid}")]
        public async Task<IActionResult> UpdateVehicle(Guid customerId, Guid id, UpdateVehicleRequest request)
        {
            ErrorOr<Vehicle> requestToVehicleResult = _vehicleService.From(customerId, id, request);

            if (requestToVehicleResult.IsError)
            {
                return Problem(requestToVehicleResult.Errors);
            }

            Vehicle vehicle = requestToVehicleResult.Value;

            ErrorOr<Updated> updateVehicleResult = await _vehicleService.UpdateVehicleAsync(vehicle);

            return updateVehicleResult.Match(
                updated => NoContent(),
                errors => Problem(errors));
        }

        [HttpDelete("{customerId:guid}/{id:guid}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            ErrorOr<Deleted> deleteVehicleResult = await _vehicleService.DeleteVehicleAsync(id);

            return deleteVehicleResult.Match(
                deleted => NoContent(),
                errors => Problem(errors));
        }

        private static VehicleResponse MapVehicleResponse(Vehicle vehicle)
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
                vehicle.Category,
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered,
                vehicle.IsPublished,
                vehicle.IsOrdered);
        }
    }
}
