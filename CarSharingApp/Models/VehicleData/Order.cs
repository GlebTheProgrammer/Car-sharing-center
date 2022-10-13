namespace CarSharingApp.Models.VehicleData
{
    public class Order
    {
        public int Id { get; set; }
        public bool IsExpired { get; set; }
        public DateTime TimeItWasMade { get; set; }
    }
}
