using CarSharingApp.Models.OrderData;
using CarSharingApp.Models.RatingData;
using CarSharingApp.Repository.Interfaces;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository
{
    public class OrdersLocalRepository : IOrdersRepository
    {
        private List<OrderModel> orders;

        private void SetUpLocalRepository()
        {
            string filePath = "~/../Repository/LocalRepository/Data/OrdersData.json";

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                orders = JsonSerializer.Deserialize<List<OrderModel>>(jsonString)!;
            }
            else
            {
                orders = new List<OrderModel>();
            }
        }

        public int GetNumberOfVehicleOrders(int vehicleId)
        {
            if (orders == null)
                SetUpLocalRepository();

            return orders.Where(order => order.OrderedVehicleId == vehicleId).Count();
        }

        public int GetNumberOfActiveOrdersForAUser(int userId)
        {
            if (orders == null)
                SetUpLocalRepository();

            return orders.Where(order => order.IsActive && order.OrderedUserId == userId).Count();
        }

        public List<OrderModel> GetActiveOrdersForAUser(int userId)
        {
            if (orders == null)
                SetUpLocalRepository();

            return orders.Where(order => order.IsActive && order.OrderedUserId == userId).ToList();
        }
    }
}
