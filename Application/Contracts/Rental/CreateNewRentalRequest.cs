namespace CarSharingApp.Application.Contracts.Rental
{
    public record CreateNewRentalRequest(
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
