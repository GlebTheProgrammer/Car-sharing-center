namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record UpdateVehicleStatusRequest(
        string vehicleId,

        bool IsOrdered,
        bool IsPublished,
        bool IsConfirmedByAdmin
    );
}
