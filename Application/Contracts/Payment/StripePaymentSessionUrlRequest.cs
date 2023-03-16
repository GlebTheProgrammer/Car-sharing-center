namespace CarSharingApp.Application.Contracts.Payment
{
    public sealed record StripePaymentSessionUrlRequest(
        string VehicleId,
        string VehicleOwnerId,
        string VehicleName,
        string Amount,
        decimal TariffPerHour,
        decimal TariffPerDay,
        string RentalStartsDateTimeUTC,
        string RentalEndsDateTimeUTC,
        string SuccessUrl,
        string CancelationUrl
    );
}
