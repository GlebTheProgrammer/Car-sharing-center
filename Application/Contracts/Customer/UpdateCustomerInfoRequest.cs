namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record UpdateCustomerInfoRequest(
        string FirstName,
        string LastName,
        string StreetAddress,
        string AptSuiteEtc,
        string City,
        string Country,
        string ZipPostCode,
        string PhoneNumber,
        string DriverLicenseIdentifier,
        bool HasAcceptedNewsSharing,
        string ProfileDescription,
        string ProfileImage
    );
}
