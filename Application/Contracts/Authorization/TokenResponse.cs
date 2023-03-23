using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Authorization
{
    public sealed record TokenResponse(
        [property: JsonPropertyName("jwToken")] string JWToken,
        [property: JsonPropertyName("customerImage")] string CustomerImage
    );
}
