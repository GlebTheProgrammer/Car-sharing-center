namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record UpdateVehicleStatusRequest(
        bool IsOrdered,
        bool IsPublished,
        bool IsConfirmedByAdmin
    );
}
