using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record NearbyVehiclesDisplayOnMapResponse(
        [property: JsonPropertyName("vehicles")] List<NearbyVehicleDisplayOnMapResponse> Vehicles
    );

    public record NearbyVehicleDisplayOnMapResponse(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("latitude")] string Latitude,
        [property: JsonPropertyName("longitude")] string Longitude,
        [property: JsonPropertyName("hourlyRentalPrice")] string HourlyRentalPrice,
        [property: JsonPropertyName("dailyRentalPrice")] string DailyRentalPrice,
        [property: JsonPropertyName("timesOrdered")] int TimesOrdered,
        [property: JsonPropertyName("distance")] string Distance
    );
}
