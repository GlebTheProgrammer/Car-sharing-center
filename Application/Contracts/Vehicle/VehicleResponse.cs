using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public sealed record VehicleResponse(
        Guid Id,
        Guid CustomerId,
        string Name,
        string Image,
        string BriefDescription,
        string Description,
        Tariff Tariff,
        Location Location,
        Specifications Specifications,
        List<string> Categories,
        int TimesOrdered,
        DateTime PublishedTime,
        DateTime? LastTimeOrdered,
        Status Status
    );
}
