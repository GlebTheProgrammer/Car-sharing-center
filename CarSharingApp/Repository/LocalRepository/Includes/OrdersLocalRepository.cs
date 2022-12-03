using CarSharingApp.Models.OrderData;
using CarSharingApp.Repository.Interfaces.Includes;
using System.Text.Json;

namespace CarSharingApp.Repository.LocalRepository.Includes
{
    public sealed class OrdersLocalRepository : IOrdersRepository
    {
        private const string filePath = "~/../Repository/LocalRepository/Data/OrdersData.json";

        private List<OrderModel> orders;

        public OrdersLocalRepository()
        {
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
            return orders.Where(order => order.OrderedVehicleId == vehicleId).Count();
        }

        public int GetNumberOfActiveOrdersForAUser(int userId)
        {
            return orders.Where(order => order.IsActive && order.OrderedUserId == userId).Count();
        }

        public List<OrderModel> GetActiveOrdersForAUser(int userId)
        {
            return orders.Where(order => order.IsActive && order.OrderedUserId == userId).ToList();
        }

        public async void AddNewOrder(OrderModel newOrder)
        {
            if (orders.Count == 0)
                newOrder.Id = 0;
            else
                newOrder.Id = orders.Max(order => order.Id) + 1;

            orders.Add(newOrder);

            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, orders, options);
            await createStream.DisposeAsync();
        }

        public OrderModel GetOrderById(int orderId)
        {
            return orders.First(order => order.Id == orderId);
        }

        public async void FinishOrder(int orderId)
        {
            int replaceIndex = orders.IndexOf(orders.First(order => order.Id == orderId));

            orders[replaceIndex].IsActive = false;

            await SaveChanges();
        }

        public async Task<List<int>> CheckExpiredOrdersAndGetVehiclesId()
        {
            List<int> result = new List<int>();
            DateTime now = DateTime.Now;

            var activeOrders = orders.Where(order => order.IsActive).ToList();

            foreach (var order in activeOrders)
            {
                if (order.ExpiredTime < now)
                {
                    int orderIndex = orders.IndexOf(orders.First(listOrder => listOrder == order));

                    result.Add(orders[orderIndex].OrderedVehicleId);
                    orders[orderIndex].IsActive = false;
                }
            }

            if (result.Count != 0)
                await SaveChanges();

            return result;
        }

        public DateTime GetLastOrderExpiredDate(int orderedVehicleId)
        {
            if (orders.FirstOrDefault(order => order.OrderedVehicleId == orderedVehicleId) == null)
                return DateTime.Now;
            else
                return orders.Where(order => order.OrderedVehicleId == orderedVehicleId).Max(order => order.OrderMadeTime);
        }

        public async void DeleteAllVehicleOrders(int vehicleId)
        {
            orders.RemoveAll(order => order.OrderedVehicleId == vehicleId);

            await SaveChanges();
        }
    }
}
