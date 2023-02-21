using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Authorization
{
    public record TokenResponse(
        [property: JsonPropertyName("jwToken")] string JWToken
    );
}
