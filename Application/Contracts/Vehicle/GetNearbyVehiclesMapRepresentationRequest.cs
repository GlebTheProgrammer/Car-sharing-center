namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record GetNearbyVehiclesMapRepresentationRequest(
        string UserLatitude,
        string UserLongitude,
        int Count
    );
}
