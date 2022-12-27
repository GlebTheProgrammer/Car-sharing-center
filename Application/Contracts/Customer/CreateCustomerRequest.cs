namespace CarSharingApp.Application.Contracts.Customer
{
    public record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string Country,
        string City,
        string Address,
        string PhoneNumber,
        string DriverLicenseIdentifier,
        string Postcode,
        bool HasAcceptedNewsSharing,
        string login,
        string email,
        string password);
}
