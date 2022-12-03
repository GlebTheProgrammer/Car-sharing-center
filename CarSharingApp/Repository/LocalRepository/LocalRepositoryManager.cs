using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Repository.Interfaces.Includes;
using CarSharingApp.Repository.LocalRepository.Includes;

namespace CarSharingApp.Repository.LocalRepository
{
    public sealed class LocalRepositoryManager : IRepositoryManager
    {
        public IClientsRepository ClientsRepository { get; private set; }
        public IVehiclesRepository VehiclesRepository { get; private set; }
        public IRatingsRepository RatingRepository { get; private set; }
        public IOrdersRepository OrdersRepository { get; private set; }

        public LocalRepositoryManager()
        {
            VehiclesRepository = new VehiclesLocalRepository();
            ClientsRepository = new ClientsLocalRepository();
            RatingRepository = new RatingsLocalRepository();
            OrdersRepository = new OrdersLocalRepository();
        }
    }
}
