using CarSharingApp.Models.OrderData;
using CarSharingApp.Models.RatingData;
using CarSharingApp.Models.VehicleData;
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

        public async void AddNewOrder(OrderModel newOrder)
        {
            if (orders == null)
                SetUpLocalRepository();

            if (orders.Count == 0)
                newOrder.Id = 0;
            else
                newOrder.Id = orders.Max(order => order.Id) + 1;

            orders.Add(newOrder);

            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            string filePath = "~/../Repository/LocalRepository/Data/OrdersData.json";

            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, orders, options);
            await createStream.DisposeAsync();
        }

        public OrderModel GetOrderById(int orderId)
        {
            if (orders == null)
                SetUpLocalRepository();

            return orders.First(order => order.Id == orderId);
        }

        public async void FinishOrder(int orderId)
        {
            int replaceIndex = orders.IndexOf(orders.First(order => order.Id == orderId));

            orders[replaceIndex].IsActive = false;

            await SaveChanges();
        }
    }
}
