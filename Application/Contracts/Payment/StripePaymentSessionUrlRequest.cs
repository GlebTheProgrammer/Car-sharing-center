namespace CarSharingApp.Application.Contracts.Payment
{
    public sealed record StripePaymentSessionUrlRequest(
        string VehicleId,
        string VehicleOwnerId,
        string VehicleName,
        string Amount,
        decimal TariffPerHour,
        decimal TariffPerDay,
        DateTime RentalStartsDateTimeUTC,
        DateTime RentalEndsDateTimeUTC,
        string SuccessUrl,
        string CancelationUrl
    );
}
