using CarSharingApp.Domain.Enums;
using CarSharingApp.Domain.SmartEnums;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record CreateVehicleRequest(
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
        string Drivetrain,
        string FuelType,
        string Transmission,
        string Engine,
        string VIN,
        Categories Category
    );
        
}
