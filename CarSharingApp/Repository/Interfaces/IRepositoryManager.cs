using CarSharingApp.Repository.Interfaces.Includes;

namespace CarSharingApp.Repository.Interfaces
{
    public interface IRepositoryManager
    {
        public IClientsRepository ClientsRepository { get; }
        public IVehiclesRepository VehiclesRepository { get; }
        public IRatingsRepository RatingRepository { get; }
        public IOrdersRepository OrdersRepository { get; }
    }
}
