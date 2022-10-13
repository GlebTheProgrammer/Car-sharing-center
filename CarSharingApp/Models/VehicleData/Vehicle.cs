namespace CarSharingApp.Models.VehicleData
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int MaxSpeed { get; set; }

        public List<int> OrdersHistroy { get; set; } // История заказов автомобиля
        public int RateId { get; set; } // Тариф
        public int LocationId { get; set; }
    }
}
