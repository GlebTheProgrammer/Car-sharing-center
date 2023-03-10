using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<Created> CreateVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetAllAsync();
        Task<List<Vehicle>> GetWithLimitAsync(int skip, int limit);
        Task<List<Vehicle>> GetAllCustomerVehiclesAsync(Guid customerId);
        Task<int> GetRecordsCountAsync();
        Task<List<Vehicle>> GetWithLimitPublishedAndApprovedAsync(int skip, int limit);
        Task<ErrorOr<Vehicle>> GetVehicleAsync(Guid id);
        Task<Updated> UpdateVehicleAsync(Vehicle vehicle);
        Task<Updated> UpdateVehicleStatusAsync(Vehicle vehicle);
        Task<Deleted> DeleteVehicleAsync(Guid id);

        ErrorOr<Vehicle> From(Guid customerId, CreateVehicleRequest request);
        ErrorOr<Vehicle> From(Vehicle vehicle, UpdateVehicleRequest request);
        ErrorOr<Vehicle> From(Vehicle vehicle, UpdateVehicleStatusRequest request);
    }
}
