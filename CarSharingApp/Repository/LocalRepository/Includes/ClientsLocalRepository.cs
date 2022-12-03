using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.ClientData.Includes;
using CarSharingApp.Repository.Interfaces.Includes;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository.Includes
{
    public sealed class ClientsLocalRepository : IClientsRepository
    {
        private const string filePath = "~/../Repository/LocalRepository/Data/ClientsData.json";

        private List<ClientModel> clients;

        public ClientsLocalRepository()
        {
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

        public async void AddNewClient(ClientModel client)
        {
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
            return clients;
        }

        public ClientModel GetClientById(int id)
        {
            return clients.First(vehicle => vehicle.Id == id);
        }

        public async Task SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, clients, options);
            await createStream.DisposeAsync();
        }

        public ClientModel TrySignIn(string email, string password)
        {
            string hashPassword = GetPasswordHash(password);

            var signInUser = clients.FirstOrDefault(client => client.Email == email && client.Password == hashPassword);

            return signInUser;
        }

        public string GetClientUsername(int id)
        {
            return clients.First(client => client.Id == id).Username;
        }

        public async void UpdateClient(ClientModel client)
        {
            int replaceIndex = clients.IndexOf(clients.First(prevClient => prevClient.Id == client.Id));

            clients[replaceIndex] = client;

            await SaveChanges();
        }

        public async void UpdateClientPassword(int clientId, string clientNewPassword)
        {
            string clientNewHashPassword = GetPasswordHash(clientNewPassword);

            var client = clients.First(client => client.Id == clientId);

            client.Password = clientNewHashPassword;

            await SaveChanges();
        }

        public async void IncreaseClientsVehiclesSharedAndOrderedCount(int clientId, int orderedCarOwnerId)
        {
            int carOwnerIndex = clients.IndexOf(clients.First(client => client.Id == orderedCarOwnerId));
            int orderedClientIndex = clients.IndexOf(clients.First(client => client.Id == clientId));

            clients[carOwnerIndex].VehiclesShared += 1;
            clients[orderedClientIndex].VehiclesOrdered += 1;

            await SaveChanges();
        }





        private string GetPasswordHash(string password, string salt = "")
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);

                byte[] hashBytes = sha.ComputeHash(passwordBytes);

                string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hash;
            }
        }
    }
}
