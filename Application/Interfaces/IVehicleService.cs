using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Interfaces
{
    public interface IVehicleService
    {
        Task<ErrorOr<Created>> CreateVehicleAsync(Vehicle vehicle);
        Task<ErrorOr<List<Vehicle>>> GetAllAsync();
        Task<ErrorOr<Vehicle>> GetVehicleAsync(Guid id);
        Task<ErrorOr<Updated>> UpdateVehicleAsync(Vehicle vehicle);
        Task<ErrorOr<Deleted>> DeleteVehicleAsync(Guid id);

        ErrorOr<Vehicle> From(Guid customerId, CreateVehicleRequest request);
        ErrorOr<Vehicle> From(Guid customerId, Guid id, UpdateVehicleRequest request);
    }
}
