namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record GetVehiclesByCriteriaRequest(
        string MaxHourlyRentalPrice,
        string MaxDailyRentalPrice,
        string? Country,
        string? Categories,
        string? ExteriorColor,
        string? InteriorColor,
        string? Drivetrain,
        string? FuelType,
        string? Transmission,
        string? Engine,
        bool SearchAllVehicles,
        bool SearchMyVehicles,
        bool SearchAllExceptMyVehicles
    );
}
