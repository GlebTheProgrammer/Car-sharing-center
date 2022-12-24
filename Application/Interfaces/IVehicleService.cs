using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Application.Interfaces
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetVehicleAsync(Guid id);
        Task UpsertVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(Guid id);
    }
}
