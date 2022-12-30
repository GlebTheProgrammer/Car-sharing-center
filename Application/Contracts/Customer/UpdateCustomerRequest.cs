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
        string Login,
        string Email,
        string Password,
        string? ProfileDescription = null,
        string? ProfileImage = null,
        int? VehiclesOrdered = null,
        int? VehiclesShared = null,
        bool? IsOnline = null
    );
}
