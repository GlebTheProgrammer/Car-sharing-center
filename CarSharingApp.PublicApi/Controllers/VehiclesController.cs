using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.PublicApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("{customerId:guid}")]
        public async Task<IActionResult> CreateVehicle(Guid customerId, CreateVehicleRequest request)
        {
            var vehicle = new Vehicle(
                Guid.NewGuid(),
                customerId,
                request.Name,
                request.Image,
                request.BriefDescription,
                request.Description,
                request.Tariff,
                request.Location,
                timesOrdered: 0,
                publishedTime: DateTime.Now,
                lastTimeOrdered: null,
                isPublished: false,
                isOrdered: false,
                request.Specifications);

            await _vehicleService.AddVehicleAsync(vehicle);

            var response = new VehicleResponse(
                vehicle.Id,
                vehicle.CustomerId,
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                vehicle.Tariff,
                vehicle.Location,
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered,
                vehicle.IsPublished,
                vehicle.IsOrdered,
                vehicle.Specifications);

            return CreatedAtAction(
                actionName: nameof(GetVehicle),
                routeValues: new { id = vehicle.Id },
                value: response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVehicle(Guid id)
        {
            Vehicle vehicle = await _vehicleService.GetVehicleAsync(id);

            var response = new VehicleResponse(
                vehicle.Id,
                vehicle.CustomerId,
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                vehicle.Tariff,
                vehicle.Location,
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered,
                vehicle.IsPublished,
                vehicle.IsOrdered,
                vehicle.Specifications);

            return Ok(response);
        }

        [HttpPut("{customerId:guid}/{id:guid}")]
        public async Task<IActionResult> UpsertVehicle(Guid customerId, Guid id, UpsertVehicleRequest request)
        {
            var vehicle = new Vehicle(
                id,
                customerId,
                request.Name,
                request.Image,
                request.BriefDescription,
                request.Description,
                request.Tariff,
                request.Location,
                timesOrdered: 0,
                publishedTime: DateTime.Now,
                lastTimeOrdered: null,
                isPublished: false,
                isOrdered: false,
                request.Specifications);

            await _vehicleService.UpsertVehicleAsync(vehicle);

            // TODO: return 201 if a new vehicle was created

            return NoContent();
        }

        [HttpDelete("{customerId:guid}/{id:guid}")]
        public IActionResult DeleteVehicle(Guid id)
        {
            _vehicleService.DeleteVehicleAsync(id);

            return NoContent();
        }
    }
}
