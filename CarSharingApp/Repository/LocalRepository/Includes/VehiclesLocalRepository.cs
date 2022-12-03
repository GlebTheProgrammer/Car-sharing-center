using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces.Includes;
using System.Text;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository.Includes
{
    public class VehiclesLocalRepository : IVehiclesRepository
    {
        private const string filePath = "~/../Repository/LocalRepository/Data/VehiclesData.json";
        private const string imagesDirectoryPath = "wwwroot/vehicleImages/";

        private List<VehicleModel> vehicles;

        public VehiclesLocalRepository()
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                vehicles = JsonSerializer.Deserialize<List<VehicleModel>>(jsonString)!;
            }
            else
            {
                vehicles = new List<VehicleModel>();
            }
        }

        public IEnumerable<VehicleModel> GetAllVehiclesForCatalog()
        {
            return vehicles.Where(vehicle => vehicle.IsPublished && !vehicle.IsOrdered);
        }

        public VehicleModel GetVehicleById(int id)
        {
            return vehicles.First(vehicle => vehicle.Id == id);
        }

        public async void ShareNewVehicle(VehicleModel vehicleModel)
        {

            if (vehicles.Count == 0)
                vehicleModel.Id = 0;
            else
                vehicleModel.Id = vehicles.Max(vehicle => vehicle.Id) + 1;

            vehicles.Add(vehicleModel);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, vehicles, options);
            await createStream.DisposeAsync();
        }

        public IEnumerable<VehicleModel> GetAllActiveUserVehicles(int userId)
        {
            return vehicles.Where(vehicle => vehicle.IsPublished && vehicle.OwnerId == userId);
        }

        public IEnumerable<VehicleModel> GetAllUserVehicles(int userId)
        {
            return vehicles.Where(vehicle => vehicle.OwnerId == userId);
        }

        public async void PublishVehicleInTheCatalog(int vehicleId)
        {
            vehicles.First(vehicle => vehicle.Id == vehicleId).IsPublished = true;
            await SaveChanges();
        }

        public async void RemoveVehicleFromTheCatalog(int vehicleId)
        {
            vehicles.First(vehicle => vehicle.Id == vehicleId).IsPublished = false;
            await SaveChanges();
        }

        public async void DeleteVehicle(int vehicleId)
        {
            var vehicleToDelete = vehicles.First(vehicle => vehicle.Id == vehicleId);
            var vehicleImagePath = string.Concat(imagesDirectoryPath, vehicleToDelete.Image);

            if (File.Exists(vehicleImagePath))
                File.Delete(vehicleImagePath);

            vehicles.Remove(vehicleToDelete);
            await SaveChanges();
        }

        public async void UpdateVehicle(VehicleModel vehicle)
        {
            int replaceIndex = vehicles.IndexOf(vehicles.First(prevVehicle => prevVehicle.Id == vehicle.Id));

            vehicles[replaceIndex] = vehicle;

            await SaveChanges();
        }

        public async void ChangeVehicleIsOrderedState(int vehicleId, bool state)
        {
            int replaceIndex = vehicles.IndexOf(vehicles.First(vehicle => vehicle.Id == vehicleId));

            vehicles[replaceIndex].IsOrdered = state;

            if (state)
                vehicles[replaceIndex].TimesOrdered += 1;

            await SaveChanges();
        }

        public async void ChangeVehiclesIsOrderedState(List<int> vehicleIds, bool state)
        {
            for (int i = 0; i < vehicleIds.Count; i++)
            {
                vehicles[vehicles.IndexOf(vehicles.First(vehicle => vehicle.Id == vehicleIds[i]))].IsOrdered = false;
            }

            await SaveChanges();
        }
    }
}
