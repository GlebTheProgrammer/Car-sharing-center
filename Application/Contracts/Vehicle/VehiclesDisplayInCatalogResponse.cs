using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record VehiclesDisplayInCatalogResponse(
        [property: JsonPropertyName("vehicles")] List<VehicleDisplayInCatalog> Vehicles
    );

    public sealed record VehicleDisplayInCatalog(
        [property: JsonPropertyName("vehicleId")] string VehicleId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("briefDescription")] string BriefDescription,
        [property: JsonPropertyName("hourlyRentalPrice")] string HourlyRentalPrice,
        [property: JsonPropertyName("dailyRentalPrice")] string DailyRentalPrice,
        [property: JsonPropertyName("timesOrdered")] int TimesOrdered
    );
}
