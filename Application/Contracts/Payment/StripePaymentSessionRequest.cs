namespace CarSharingApp.Application.Contracts.Payment
{
    public sealed class StripePaymentSessionRequest 
    {
        public string VehicleId { get; set; } = null!;
        public string VehicleOwnerId { get; set; } = null!;
        public string VehicleName { get; set; } = null!;
        public string Amount { get; set; } = null!;
        public decimal TariffPerHour { get; set; }
        public decimal TariffPerDay { get; set; }

        public string RentalStartsDateTimeLocalStr { get; set; } = null!;
        public string RentalEndsDateTimeLocalStr { get; set; } = null!;
    }
}
