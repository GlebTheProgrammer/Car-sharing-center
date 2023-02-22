namespace CarSharingApp.Application.Contracts.Payment
{
    public class StripePaymentSessionRequest 
    {
        public string VehicleId { get; set; } = null!;
        public string VehicleOwnerId { get; set; } = null!;
        public string VehicleName { get; set; } = null!;
        public string Amount { get; set; } = null!;
        public decimal TariffPerHour { get; set; }
        public decimal TariffPerDay { get; set; }

        public string StartHour { get; set; } = null!;
        public string StartDay { get; set; } = null!;
        public string StartMonth { get; set; } = null!;

        public string EndHour { get; set; } = null!;
        public string EndDay { get; set; } = null!;
        public string EndMonth { get; set; } = null!;
    }
}
