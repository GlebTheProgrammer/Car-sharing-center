using CarSharingApp.Domain.Entities;

namespace CarSharingApp.Application.Interfaces
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetAllAsync();
    }
}
