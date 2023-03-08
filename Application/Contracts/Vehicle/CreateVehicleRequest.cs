namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record CreateVehicleRequest(
        string Name, 
        string Image,
        string BriefDescription,
        string Description,
        decimal HourlyRentalPrice,
        decimal DailyRentalPrice,
        string StreetAddress,
        string AptSuiteEtc,
        string City,
        string Country,
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
        string Categories
    );
        
}
