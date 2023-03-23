using CarSharingApp.Domain.ValueObjects;

namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record CustomerResponse(
        Guid Id,
        string FirstName,
        string LastName,
        Address Address,
        string PhoneNumber,
        string DriverLicenseIdentifier,
        Profile Profile,
        Statistics Statistics,
        bool HasAcceptedNewsSharing,
        Credentials Credentials
    );
}
