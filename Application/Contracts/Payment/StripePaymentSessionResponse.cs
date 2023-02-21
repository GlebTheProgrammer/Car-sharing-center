using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Payment
{
    public record StripePaymentSessionResponse(
        [property: JsonPropertyName("sessionUrl")] string SessionUrl
    );
}
