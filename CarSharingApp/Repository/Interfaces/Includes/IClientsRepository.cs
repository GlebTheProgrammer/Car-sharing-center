using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Repository.Interfaces.Includes
{
    public interface IClientsRepository
    {
        public IEnumerable<ClientModel> GetAllClients();
        public ClientModel GetClientById(int id);
        public string GetClientUsername(int id);
        public void AddNewClient(ClientModel client);
        public Task SaveChanges();
        public ClientModel? TrySignIn(string email, string password);
        public void UpdateClient(ClientModel client);
        public void UpdateClientPassword(int clientId, string clientNewPassword);
        public void IncreaseClientsVehiclesSharedAndOrderedCount(int clientId, int orderedCarOwnerId);
    }
}
