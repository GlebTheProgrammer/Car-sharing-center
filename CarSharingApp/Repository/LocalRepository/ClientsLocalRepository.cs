using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.ClientData.Includes;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository
{
    public class ClientsLocalRepository : IClientsRepository
    {
        private List<ClientModel> clients;

        private void SetUpLocalRepository()
        {
            string filePath = "~/../Repository/LocalRepository/Data/ClientsData.json";

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                clients = JsonSerializer.Deserialize<List<ClientModel>>(jsonString)!;
            }
            else
            {
                clients = new List<ClientModel>();
            }
        }

        private string GetPasswordHash(string password, string salt = "")
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using(var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);

                byte[] hashBytes = sha.ComputeHash(passwordBytes);

                string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hash;
            }
        }







        public async void AddNewClient(ClientModel client)
        {
            if (clients == null)
                SetUpLocalRepository();

            if (clients.Count == 0)
                client.Id = 0;
            else
                client.Id = clients.Max(client => client.Id) + 1;

            client.Role = Role.Client;
            client.Password = GetPasswordHash(client.Password);

            clients.Add(client);
            await SaveChanges();
        }

        public IEnumerable<ClientModel> GetAllClients()
        {
            if (clients == null)
                SetUpLocalRepository();

            return clients;
        }

        public ClientModel GetClientById(int id)
        {
            if (clients == null)
                SetUpLocalRepository();

            return clients.First(vehicle => vehicle.Id == id);
        }

        public async Task SaveChanges()
        {
            string filePath = "~/../Repository/LocalRepository/Data/ClientsData.json";

            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, clients, options);
            await createStream.DisposeAsync();
        }

        public ClientModel TrySignIn(string email, string password)
        {
            if (clients == null)
                SetUpLocalRepository();

            string hashPassword = GetPasswordHash(password);


            var signInUser = clients.FirstOrDefault(client => client.Email == email && client.Password == hashPassword);

            return signInUser;
        }

        public string GetClientUsername(int id)
        {
            if (clients == null)
                SetUpLocalRepository();

            return clients.First(client => client.Id == id).Username;
        }
    }
}
