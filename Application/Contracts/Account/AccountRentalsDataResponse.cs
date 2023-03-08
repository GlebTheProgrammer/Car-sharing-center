using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Account
{
    public sealed record AccountRentalsDataResponse(
        [property: JsonPropertyName("rentals")] List<AccountRentalData> Rentals
    );

    public sealed record AccountRentalData(
        [property: JsonPropertyName("rentalId")] string RentalId,
        [property: JsonPropertyName("vehicleName")] string VehicleName,
        [property: JsonPropertyName("vehicleImage")] string VehicleImage,
        [property: JsonPropertyName("amount")] string Amount,
        [property: JsonPropertyName("rentalMadeDateTime")] DateTime RentalMadeDateTime,
        [property: JsonPropertyName("startsDateTime")] DateTime StartsDateTime,
        [property: JsonPropertyName("timeLeftInMinutes")] int TimeLeftInMinutes,
        [property: JsonPropertyName("expiresDateTime")] DateTime ExpiresDateTime,
        [property: JsonPropertyName("isActive")] bool IsActive
    );
}
