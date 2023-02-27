using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Account
{
    public record AccountRentalsDataResponse(
        [property: JsonPropertyName("rentals")] List<AccountRentalData> Rentals
    );

    public record AccountRentalData(
        [property: JsonPropertyName("rentalId")] string RentalId,
        [property: JsonPropertyName("vehicleName")] string VehicleName,
        [property: JsonPropertyName("vehicleImage")] string VehicleImage,
        [property: JsonPropertyName("amount")] string Amount,
        [property: JsonPropertyName("rentedDateTime")] DateTime RentedDateTime,
        [property: JsonPropertyName("timeLeftInMinutes")] int TimeLeftInMinutes,
        [property: JsonPropertyName("expiresDateTime")] DateTime ExpiresDateTime,
        [property: JsonPropertyName("isActive")] bool IsActive
    );
}
