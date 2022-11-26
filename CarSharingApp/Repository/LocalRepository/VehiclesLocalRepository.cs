using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using System.Text;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository
{
    public class VehiclesLocalRepository : IVehiclesRepository
    {
        private List<VehicleModel> vehicles;


        private void SetUpLocalRepository()
        {
            string filePath = "~/../Repository/LocalRepository/Data/VehiclesData.json";

            if(File.Exists(filePath))
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
            if (vehicles == null)
                SetUpLocalRepository();

            // Возвращаем только те автомобили, которые были опубликованы пользователем в его личном кабинете, а также те, которые не заказаны другим пользователем в нынешний момент
            return vehicles.Where(vehicle => vehicle.IsPublished && !vehicle.IsOrdered);
        }

        public VehicleModel GetVehicleById(int id)
        {
            if (vehicles == null)
                SetUpLocalRepository();

            return vehicles.First(vehicle => vehicle.Id == id);
        }

        public async void ShareNewVehicle(VehicleModel vehicleModel)
        {
            if (vehicles == null)
                SetUpLocalRepository();

            if (vehicles.Count == 0)
                vehicleModel.Id = 0;
            else
                vehicleModel.Id = vehicles.Max(vehicle => vehicle.Id) + 1;

            vehicles.Add(vehicleModel);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            string filePath = "~/../Repository/LocalRepository/Data/VehiclesData.json";

            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, vehicles, options);
            await createStream.DisposeAsync();
        }

        public IEnumerable<VehicleModel> GetAllActiveUserVehicles(int userId)
        {
            if (vehicles == null)
                SetUpLocalRepository();

            return vehicles.Where(vehicle => vehicle.IsPublished && vehicle.OwnerId == userId);
        }

        public IEnumerable<VehicleModel> GetAllUserVehicles(int userId)
        {
            if (vehicles == null)
                SetUpLocalRepository();

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
            var vehicleImagePath = "wwwroot" + vehicleToDelete.Image;

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
            if (vehicles == null)
                SetUpLocalRepository();

            for (int i = 0; i < vehicleIds.Count; i++)
            {
                vehicles[vehicles.IndexOf(vehicles.First(vehicle => vehicle.Id == vehicleIds[i]))].IsOrdered = false;
            }

            await SaveChanges();
        }
    }
}
