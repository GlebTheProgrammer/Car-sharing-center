using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record VehicleInformationResponse(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("customerLogin")] string CustomerLogin,
        [property: JsonPropertyName("customerId")] string CustomerId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("image")] string Image,
        [property: JsonPropertyName("briefDescription")] string BriefDescription,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("hourlyRentalPrice")] string HourlyRentalPrice,
        [property: JsonPropertyName("dailyRentalPrice")] string DailyRentalPrice,
        [property: JsonPropertyName("streetAddress")] string StreetAddress,
        [property: JsonPropertyName("aptSuiteEtc")] string AptSuiteEtc,
        [property: JsonPropertyName("city")] string City,
        [property: JsonPropertyName("country")] string Country,
        [property: JsonPropertyName("latitude")] string Latitude,
        [property: JsonPropertyName("longitude")] string Longitude,
        [property: JsonPropertyName("productionYear")] int ProductionYear,
        [property: JsonPropertyName("maxSpeedKph")] int MaxSpeedKph,
        [property: JsonPropertyName("exteriorColour")] string ExteriorColour,
        [property: JsonPropertyName("interiorColour")] string InteriorColour,
        [property: JsonPropertyName("drivetrain")] string Drivetrain,
        [property: JsonPropertyName("fuelType")] string FuelType,
        [property: JsonPropertyName("transmission")] string Transmission,
        [property: JsonPropertyName("engine")] string Engine,
        [property: JsonPropertyName("vin")] string Vin,
        [property: JsonPropertyName("categories")] List<string> Categories,
        [property: JsonPropertyName("timesOrdered")] int TimesOrdered,
        [property: JsonPropertyName("publishedTime")] DateTime PublishedTime,
        [property: JsonPropertyName("lastTimeOrdered")] DateTime? LastTimeOrdered,
        [property: JsonPropertyName("isOfferedCustomerVehicle")] bool IsOfferedCustomerVehicle
    );
}
