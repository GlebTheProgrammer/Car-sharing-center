namespace CarSharingApp.Application.Contracts.Payment
{
    public record StripePaymentSessionUrlRequest(
        string VehicleId,
        string VehicleOwnerId,
        string VehicleName,
        string Amount,
        decimal TariffPerHour,
        decimal TariffPerDay,
        string StartHour,
        string StartDay,
        string StartMonth,
        string EndHour,
        string EndDay,
        string EndMonth,
        string SuccessUrl,
        string CancelationUrl
    );
}
