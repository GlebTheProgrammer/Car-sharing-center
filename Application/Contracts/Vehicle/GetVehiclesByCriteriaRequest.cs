using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record GetVehiclesByCriteriaRequest(
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
