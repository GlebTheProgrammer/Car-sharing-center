namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record CreateCustomerRequest(
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
        string Login,
        string Email,
        string Password,
        string ConfirmPassword,
        bool HasAcceptedTermsAndConditions
    );
}
