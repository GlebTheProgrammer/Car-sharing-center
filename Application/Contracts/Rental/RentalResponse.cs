using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Rental
{
    public sealed record RentalResponse(
        [property: JsonPropertyName("rentalId")] string RentalId,
        [property: JsonPropertyName("rentedCustomerId")] string RentedCustomerId,
        [property: JsonPropertyName("vehicleOwnerId")] string VehicleOwnerId,
        [property: JsonPropertyName("vehicleId")] string VehicleId,
        [property: JsonPropertyName("payment")] RentalPaymentResponse Payment,
        [property: JsonPropertyName("rentalStartsDateTime")] DateTime RentalStartsDateTime,
        [property: JsonPropertyName("rentalEndsDateTime")] DateTime RentalEndsDateTime,
        [property: JsonPropertyName("isActive")] bool IsActive
    );

    public sealed record RentalPaymentResponse(
        [property: JsonPropertyName("paymentId")] string PaymentId,
        [property: JsonPropertyName("stripeTransactionId")] string StripeTransactionId,
        [property: JsonPropertyName("paymentDateTime")] DateTime PaymentDateTime,
        [property: JsonPropertyName("amount")] decimal Amount
    );
}
