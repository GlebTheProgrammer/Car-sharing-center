using CarSharingApp.Domain.Enums;
using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Contracts.Vehicle
{
    public record VehicleResponse(
        Guid Id,
        Guid CustomerId,
        string Name,
        string Image,
        string BriefDescription,
        string Description,
        Tariff Tariff,
        Location Location,
        Specifications Specifications,
        Categories Category,
        int TimesOrdered,
        DateTime PublishedTime,
        DateTime? LastTimeOrdered,
        bool IsPublished,
        bool IsOrdered
    );
}
