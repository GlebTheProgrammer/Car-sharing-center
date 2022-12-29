using CarSharingApp.Domain.Enums;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record UpdateVehicleRequest(
        string Name,
        string Image,
        string BriefDescription,
        string Description,
        decimal HourlyRentalPrice,
        decimal DailyRentalPrice,
        string Address,
        string Latitude,
        string Longitude,
        int ProductionYear,
        int MaxSpeedKph,
        string ExteriorColor,
        string InteriorColor,
        Drivetrain Drivetrain,
        FuelType FuelType,
        Transmission Transmission,
        Engine Engine,
        string VIN,
        Category Category
    );
}
