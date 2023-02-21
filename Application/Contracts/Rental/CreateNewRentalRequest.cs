namespace CarSharingApp.Application.Contracts.Rental
{
    public record CreateNewRentalRequest(
        string VehicleId,
        string VehicleName,
        string VehicleOwnerId,
        decimal Amount,
        DateTime PaymentDateTime,
        DateTime RentalStartsDateTime,
        DateTime RentalEndsDateTime,
        string StripeTransactionId
    );
}
