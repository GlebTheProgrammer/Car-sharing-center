using System.Text.Json.Serialization;

namespace CarSharingApp.Application.Contracts.Account
{
    public sealed record AccountStatisticsDataResponse(
        [property: JsonPropertyName("vehiclesRentedToTheirTotalNumber")] int VehiclesRentedToTheirTotalNumber,
        [property: JsonPropertyName("vehiclesPublishedToTheirTotalNumber")] int VehiclesPublishedToTheirTotalNumber,
        [property: JsonPropertyName("vehiclesHiddenToTheirTotalNumber")] int VehiclesHiddenToTheirTotalNumber,
        [property: JsonPropertyName("rentedVehiclesToShared")] int RentedVehiclesToShared
    );
}
