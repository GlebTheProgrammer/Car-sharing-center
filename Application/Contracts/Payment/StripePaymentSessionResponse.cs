using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Payment
{
    public sealed record StripePaymentSessionResponse(
        [property: JsonPropertyName("sessionUrl")] string SessionUrl
    );
}
