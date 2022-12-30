using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.ServiceErrors;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleService(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<ErrorOr<Created>> CreateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.CreateAsync(vehicle);

            return Result.Created;
        }

        public async Task<ErrorOr<Deleted>> DeleteVehicleAsync(Guid id)
        {
            await _vehicleRepository.DeleteAsync(id);

            return Result.Deleted;
        }

        public async Task<ErrorOr<List<Vehicle>>> GetAllAsync()
        {
            var result = await _vehicleRepository.GetAllAsync();

            return result.ToList();
        }

        public async Task<ErrorOr<Vehicle>> GetVehicleAsync(Guid id)
        {
            var result = await _vehicleRepository.GetAsync(id);

            if (result != null)
                return result;
            else
                return ApplicationErrors.Vehicle.NotFound;

        }

        public async Task<ErrorOr<Updated>> UpdateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);

            return Result.Updated;
        }

        public ErrorOr<Vehicle> From(Guid customerId, CreateVehicleRequest request)
        {
            return Vehicle.Create(
                customerId,
                request.Name,
                request.Image,
                request.BriefDescription,
                request.Description,
                request.HourlyRentalPrice,
                request.DailyRentalPrice,
                request.Address,
                request.Latitude,
                request.Longitude,
                request.ProductionYear,
                request.MaxSpeedKph,
                request.ExteriorColor,
                request.InteriorColor,
                request.Drivetrain,
                request.FuelType,
                request.Transmission,
                request.Engine,
                request.VIN,
                request.Category);
        }

        public ErrorOr<Vehicle> From(Guid customerId, Guid id, UpdateVehicleRequest request)
        {
            return Vehicle.Create(
                customerId,
                request.Name,
                request.Image,
                request.BriefDescription,
                request.Description,
                request.HourlyRentalPrice,
                request.DailyRentalPrice,
                request.Address,
                request.Latitude,
                request.Longitude,
                request.ProductionYear,
                request.MaxSpeedKph,
                request.ExteriorColor,
                request.InteriorColor,
                request.Drivetrain,
                request.FuelType,
                request.Transmission,
                request.Engine,
                request.VIN,
                request.Categories,
                id);
        }
    }
}
