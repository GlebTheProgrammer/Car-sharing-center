namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record UpdateCustomerInfoRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string DriverLicenseIdentifier,
        bool HasAcceptedNewsSharing,
        string ProfileDescription,
        string ProfileImage
    );
}
