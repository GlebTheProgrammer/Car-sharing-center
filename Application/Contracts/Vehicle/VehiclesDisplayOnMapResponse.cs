using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record VehiclesDisplayOnMapResponse(
        [property: JsonPropertyName("vehicles")] List<VehicleDisplayOnMap> Vehicles
    );

    public record VehicleDisplayOnMap(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("latitude")] string Latitude,
        [property: JsonPropertyName("longitude")] string Longitude
    );
}
