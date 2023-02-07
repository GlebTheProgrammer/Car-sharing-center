namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record UpdateVehicleStatusRequest(
        string vehicleId,

        bool IsOrdered,
        bool IsPublished,
        bool IsConfirmedByAdmin
    );
}
