namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record UpdateVehicleInfoRequest(
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
        List<string> Categories
    );
}
