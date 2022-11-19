﻿using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Repository.Interfaces
{
    public interface IVehiclesRepository
    {
        public IEnumerable<VehicleModel> GetAllVehiclesForCatalog();
        public IEnumerable<VehicleModel> GetAllUserVehicles(int userId);
        public IEnumerable<VehicleModel> GetAllActiveUserVehicles(int userId);
        public VehicleModel GetVehicleById(int id);
        public void ShareNewVehicle(VehicleModel vehicleModel); // User shared his vehicle
        public Task SaveChanges();
        public void PublishVehicleInTheCatalog(int vehicleId);
        public void RemoveVehicleFromTheCatalog(int vehicleId);
        public void DeleteVehicle(int vehicleId);
        public void UpdateVehicle(VehicleModel vehicle);
    }
}
