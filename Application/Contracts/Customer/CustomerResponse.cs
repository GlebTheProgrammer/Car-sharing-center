using CarSharingApp.Domain.SmartEnums;
using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Contracts.Customer
{
    public record CustomerResponse(
        Guid id,
        string FirstName,
        string LastName,
        Country Country,
        string City,
        string Address,
        string PhoneNumber,
        string DriverLicenseIdentifier,
        string ProfileDescription,
        string ProfileImage,
        string Postcode,
        int VehiclesOrdered,
        int VehiclesShared,
        bool HasAcceptedNewsSharing,
        bool IsOnline,
        Credentials Credentials);
}
