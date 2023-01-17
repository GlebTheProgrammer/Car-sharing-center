using CarSharingApp.Domain.SmartEnums;
using ErrorOr;

namespace CarSharingApp.Domain.ValidationErrors
{
    public static partial class DomainErrors
    {
        public static class Customer
        {
            public static Error InvalidLogin => Error.Validation(
            code: "Customer.InvalidLogin.Login",
            description: $"Customer login mustn't contain whitespaces and must be at least {ValueObjects.Credentials.MinLoginLength} characters long and at most {ValueObjects.Credentials.MaxLoginLength} characters long.");

            public static Error InvalidEmail => Error.Validation(
            code: "Customer.InvalidEmail.Email",
            description: "Customer email is not valid. Email must be real and starts with lowercase letter.");

            public static Error WeakPassword => Error.Validation(
            code: "Customer.WeakPassword.Password",
            description: "Customer password is not strong enough.");

            public static Error InvalidFirstName => Error.Validation(
            code: "Customer.InvalidFirstName.FirstName",
            description: "Customer first name is not valid.");

            public static Error InvalidLastName => Error.Validation(
            code: "Customer.InvalidLastName.LastName",
            description: "Customer last name is not valid.");

            public static Error NotSupportedCountry => Error.Validation(
            code: "Customer.NotSupportedCountry.Country",
            description: $"Sam Car Sharing service is not working in this country or temporarily suspended. List of supported countries: {string.Join(", ", Country.GetPossibleEnumerations())}");

            public static Error InvalidCity => Error.Validation(
            code: "Customer.InvalidCity.City",
            description: $"Customer city must be at least {ValueObjects.Address.MinCityLength} characters long and at most {ValueObjects.Address.MaxCityLength} characters long.");

            public static Error InvalidStreetAddress => Error.Validation(
            code: "Customer.InvalidStreetAddress.StreetAddress",
            description: $"Customer street address must be at least {ValueObjects.Address.MinStreetAddressLength} characters long and at most {ValueObjects.Address.MaxStreetAddressLength} characters long.");

            public static Error InvalidAptSuiteEtc => Error.Validation(
            code: "Customer.InvalidAptSuiteEtc.AptSuiteEtc",
            description: $"Customer apartment / suite number must be at least {ValueObjects.Address.MinAptSuitEtcLength} characters long and at most {ValueObjects.Address.MaxAptSuitEtcLength} characters long.");

            public static Error InvalidPhoneNumber => Error.Validation(
            code: "Customer.InvalidPhoneNumber.PhoneNumber",
            description: "Customer phone number is not valid.");

            public static Error InvalidDriverLicenseIdentifier => Error.Validation(
            code: "Customer.InvalidDriverLicenseIdentifier.DriverLicenseIdentifier",
            description: $"Customer driver license identifier must be at least {Entities.Customer.MinDriverLicenseIdentifierLength} characters long and at most {Entities.Customer.MaxDriverLicenseIdentifierLength} characters long.");

            public static Error InvalidZipPostCode => Error.Validation(
            code: "Customer.InvalidZipPostCode.ZipPostCode",
            description: $"Customer ZIP / Postcode must be at least {ValueObjects.Address.MinZipPostCodeLength} characters long and at most {ValueObjects.Address.MaxZipPostCodeLength} characters long.");

            public static Error InvalidProfileDescription => Error.Validation(
            code: "Customer.InvalidProfileDescription.ProfileDescription",
            description: $"Customer profile description must be at least {ValueObjects.Profile.MinDescriptionLength} characters long and at most {ValueObjects.Profile.MaxDescriptionLength} characters long.");

            public static Error InvalidImageLength => Error.Validation(
            code: "Customer.InvalidImageLength.Image",
            description: $"Image must be at least {ValueObjects.Profile.MinImageLength} characters long and at most {ValueObjects.Profile.MaxImageLength} characters long.");
        }
    }
}
