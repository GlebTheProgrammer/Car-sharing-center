using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Repository.Interfaces
{
    public interface IVehiclesRepository
    {
        public IEnumerable<VehicleModel> GetAllVehicles();
        public VehicleModel GetVehicleById(int id);
        public void ShareNewVehicle(VehicleModel vehicleModel); // User shared his vehicle
        public Task SaveChanges();
    }
}
