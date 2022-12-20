namespace CarSharingApp.Payment
{
    public class PaymentUrlModel
    {
        public string VehicleId { get; set; } = null!; 

        public string Amount { get; set; } = null!;
        public string VehicleName { get; set; } = null!;

        public string StartHour { get; set; } = null!;
        public string StartDay { get; set; } = null!;
        public string StartMonth { get; set; } = null!;

        public string EndHour { get; set; } = null!;    
        public string EndDay { get; set; } = null!;
        public string EndMonth { get; set; } = null!;
    }
}
