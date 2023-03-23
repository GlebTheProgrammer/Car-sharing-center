using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.ServiceErrors;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using CarSharingApp.Domain.Enums;
using ErrorOr;

namespace CarSharingApp.Application.Services
{
    public sealed class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<ActionNote> _noteRepository;

        public VehicleService(IRepository<Vehicle> vehicleRepository, IRepository<ActionNote> noteRepository)
        {
            _vehicleRepository = vehicleRepository;
            _noteRepository = noteRepository;
        }

        public async Task<Created> CreateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.CreateAsync(vehicle);

            await _noteRepository.CreateAsync(ActionNote.AddedNewVehicleNote(vehicle.CustomerId, vehicle.Name, vehicle.Id));

            return Result.Created;
        }

        public async Task<Deleted> DeleteVehicleAsync(Guid id)
        {
            var vehicleToDelete = await _vehicleRepository.GetAsync(id);

            if (vehicleToDelete is null)
                return Result.Deleted;

            await _vehicleRepository.DeleteAsync(id);

            await _noteRepository.CreateAsync(ActionNote.DeletedVehicleNote(vehicleToDelete.CustomerId, vehicleToDelete.Name));

            return Result.Deleted;
        }

        public async Task<List<Vehicle>> GetAllAsync()
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

        public async Task<List<Vehicle>> GetWithLimitAsync(int skip, int limit)
        {
            var result = await _vehicleRepository.GetAsyncWithLimit(skip, limit);

            return result.ToList();
        }

        public async Task<int> GetRecordsCountAsync()
        {
            var result = await _vehicleRepository.GetAllAsync();

            return result.Count;
        }

        public async Task<List<Vehicle>> GetWithLimitPublishedAndApprovedAsync(int skip, int limit)
        {
            var result = await _vehicleRepository.GetAsyncWithLimit(v => v.Status.IsConfirmedByAdmin && v.Status.IsPublished, skip, limit);

            return result.ToList();
        }

        public async Task<List<Vehicle>> GetAllCustomerVehiclesAsync(Guid customerId)
        {
            var result = await _vehicleRepository.GetAllAsync(v => v.CustomerId == customerId);

            return result.ToList();
        }

        public async Task<Updated> UpdateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);

            await _noteRepository.CreateAsync(ActionNote.UpdatedVehicleNote(vehicle.CustomerId, vehicle.Name));

            return Result.Updated;
        }

        public async Task<Updated> UpdateVehicleStatusAsync(Vehicle vehicle)
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
                request.StreetAddress,
                request.AptSuiteEtc,
                request.City,
                request.Country,
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
                request.Categories.Split(',').ToList());
        }

        public ErrorOr<Vehicle> From(Vehicle vehicle, UpdateVehicleRequest request)
        {
            return Vehicle.Create(
                vehicle.CustomerId,
                vehicle.Name,
                vehicle.Image,
                request.BriefDescription,
                request.Description,
                request.HourlyRentalPrice,
                request.DailyRentalPrice,
                request.StreetAddress,
                request.AptSuiteEtc,
                request.City,
                request.Country,
                request.Latitude,
                request.Longitude,
                vehicle.Specifications.ProductionYear,
                vehicle.Specifications.MaxSpeedKph,
                vehicle.Specifications.ExteriorColor.ToString(),
                vehicle.Specifications.InteriorColor.ToString(),
                vehicle.Specifications.Drivetrain.ToString(),
                vehicle.Specifications.FuelType.ToString(),
                vehicle.Specifications.Transmission.ToString(),
                vehicle.Specifications.Engine.ToString(),
                vehicle.Specifications.VIN,
                FlagEnums.GetListFromCategories(vehicle.Categories),
                vehicle.Id,
                vehicle.Status.IsPublished,
                vehicle.Status.IsOrdered,
                vehicle.Status.IsConfirmedByAdmin,
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered);
        }

        public ErrorOr<Vehicle> From(Vehicle vehicle, UpdateVehicleStatusRequest request)
        {
            return Vehicle.Create(
                vehicle.CustomerId,
                vehicle.Name,
                vehicle.Image,
                vehicle.BriefDescription,
                vehicle.Description,
                vehicle.Tariff.HourlyRentalPrice,
                vehicle.Tariff.DailyRentalPrice,
                vehicle.Location.StreetAddress,
                vehicle.Location.AptSuiteEtc,
                vehicle.Location.City,
                vehicle.Location.Country.ToString(),
                vehicle.Location.Latitude,
                vehicle.Location.Longitude,
                vehicle.Specifications.ProductionYear,
                vehicle.Specifications.MaxSpeedKph,
                vehicle.Specifications.ExteriorColor.ToString(),
                vehicle.Specifications.InteriorColor.ToString(),
                vehicle.Specifications.Drivetrain.ToString(),
                vehicle.Specifications.FuelType.ToString(),
                vehicle.Specifications.Transmission.ToString(),
                vehicle.Specifications.Engine.ToString(),
                vehicle.Specifications.VIN,
                FlagEnums.GetListFromCategories(vehicle.Categories),
                vehicle.Id,
                request.IsPublished,
                request.IsOrdered,
                request.IsConfirmedByAdmin,
                vehicle.TimesOrdered,
                vehicle.PublishedTime,
                vehicle.LastTimeOrdered);
        }
    }
}
