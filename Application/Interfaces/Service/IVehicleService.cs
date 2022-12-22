using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Domain.Abstractions
{
    public interface IVehicleService
    {
        Task CreateNewVehicleAsync(Guid customerId, string name, string image, string briefDescription, string description, Tariff tariff, Location location, Specifications specifications);
    }
}
