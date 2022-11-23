using CarSharingApp.Models.OrderData;

namespace CarSharingApp.Repository.Interfaces
{
    public interface IOrdersRepository
    {
        public int GetNumberOfVehicleOrders(int vehicleId);
        public int GetNumberOfActiveOrdersForAUser(int userId);
        public List<OrderModel> GetActiveOrdersForAUser(int userId);
        public void AddNewOrder(OrderModel newOrder);
        public Task SaveChanges();
        public OrderModel GetOrderById(int orderId);
        public void FinishOrder(int orderId);
    }
}
