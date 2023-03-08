using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Payment
{
    public sealed record StripePaymentDetailsResponse(
        [property: JsonPropertyName("paymentId")] string PaymentId,
        [property: JsonPropertyName("amount")] long Amount,
        [property: JsonPropertyName("paymentDateTime")] DateTime PaymentDateTime
    );
}
