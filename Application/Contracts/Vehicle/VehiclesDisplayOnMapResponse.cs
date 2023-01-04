using System.Text.Json;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record VehiclesDisplayOnMapResponse(
        List<VehicleDisplayOnMap> vehicles
    );

    public record VehicleDisplayOnMap(
        string Name,
        string Image,
        string Latitude,
        string Longitude
    );

    //public class VehiclesDisplayOnMapResponseModel
    //{
    //    public List<VehicleDisplayOnMapModel> Vehicles { get; set; }
    //}

    //public class VehicleDisplayOnMapModel
    //{
    //    public string Name { get; set; }
    //    public string Image { get; set; }
    //    public string Latitude { get; set; }
    //    public string Longitude { get; set; }
    //}
}
