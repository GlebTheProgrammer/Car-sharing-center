namespace CarSharingApp.Application.Contracts.Rental
{
    public sealed record CreateNewRentalRequest(
        string VehicleId,
        string VehicleName,
        string VehicleOwnerId,
        long PaymentAmount,
        DateTime PaymentDateTime,
        DateTime RentalStartsDateTime,
        DateTime RentalEndsDateTime,
        string StripePaymentId
    );
}
