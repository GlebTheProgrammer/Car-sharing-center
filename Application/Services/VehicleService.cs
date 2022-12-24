using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleService(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.CreateAsync(vehicle);
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            var result = await _vehicleRepository.GetAllAsync();
            return result;
        }

        public async Task<Vehicle> GetVehicleAsync(Guid id)
        {
            var result = await _vehicleRepository.GetAsync(id);
            return result;
        }

        public async Task UpsertVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);
        }
    }
}
