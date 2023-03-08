using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Account
{
    public sealed record AccountVehiclesDataResponse(
        [property: JsonPropertyName("vehicles")] List<AccountVehicleData> Vehicles
    );

    public sealed record AccountVehicleData(
        [property: JsonPropertyName("vehicleId")] string VehicleId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("hourlyPrice")] string HourlyPrice,
        [property: JsonPropertyName("dailyPrice")] string DailyPrice,
        [property: JsonPropertyName("publishedTime")] DateTime PublishedTime,
        [property: JsonPropertyName("timesOrdered")] int TimesOrdered,
        [property: JsonPropertyName("isOrdered")] bool IsOrdered,
        [property: JsonPropertyName("isPublished")] bool IsPublished,
        [property: JsonPropertyName("isConfirmedByAdmin")] bool IsConfirmedByAdmin
    );
}
