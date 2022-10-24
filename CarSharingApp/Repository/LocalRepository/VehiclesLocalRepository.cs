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
            string filePath = "~/../Repository/LocalRepository/Data/Data.json";

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

        public IEnumerable<VehicleModel> GetAllVehicles()
        {
            if (vehicles == null)
                SetUpLocalRepository();

            return vehicles;
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
                vehicleModel.Id = vehicles.Max(vehicle => vehicle.Id)+1;

            vehicles.Add(vehicleModel);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            string filePath = "~/../Repository/LocalRepository/Data/Data.json";

            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, vehicles, options);
            await createStream.DisposeAsync();
        }
    }
}
