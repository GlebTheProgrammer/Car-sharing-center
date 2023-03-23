namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record GetNearbyVehiclesMapRepresentationRequest(
        string UserLatitude,
        string UserLongitude,
        int Count
    );
}
