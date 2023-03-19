using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record CustomerDataResponse(
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("phoneNumber")] string PhoneNumber,
        [property: JsonPropertyName("driverLicenseIdentifier")] string DriverLicenseIdentifier,
        [property: JsonPropertyName("profileDescription")] string ProfileDescription,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("login")] string Login,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("hasAcceptedNewsSharing")] bool HasAcceptedNewsSharing
    );
}
