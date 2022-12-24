using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record CreateVehicleRequest(
        string Name, 
        string Image,
        string BriefDescription,
        string Description,
        Tariff Tariff,
        Location Location,
        Specifications Specifications
    );
        
}
