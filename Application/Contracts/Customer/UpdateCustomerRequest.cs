namespace CarSharingApp.Application.Contracts.Customer
{
    public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string Country,
    string City,
    string Address,
    string PhoneNumber,
    string DriverLicenseIdentifier,
    string Postcode,
    bool HasAcceptedNewsSharing,
    string? ProfileDescription = null,
    string? ProfileImage = null,
    int? VehiclesOrdered = null,
    int? VehiclesShared = null,
    bool? isOnline = null);
}
