using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<Created> CreateVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> GetAllAsync();
        Task<List<Vehicle>> GetWithLimit(int skip, int limit);
        Task<int> GetRecordsCount();
        Task<List<Vehicle>> GetWithLimitPublishedAndApproved(int skip, int limit);
        Task<ErrorOr<Vehicle>> GetVehicleAsync(Guid id);
        Task<Updated> UpdateVehicleAsync(Vehicle vehicle);
        Task<Deleted> DeleteVehicleAsync(Guid id);

        ErrorOr<Vehicle> From(Guid customerId, CreateVehicleRequest request);
        ErrorOr<Vehicle> From(Vehicle vehicle, UpdateVehicleInfoRequest request);
    }
}
