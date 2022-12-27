using ErrorOr;

namespace CarSharingApp.Domain.ValidationErrors
{
    public static partial class DomainErrors
    {
        public static class Customer
        {
            public static Error InvalidUsername => Error.Validation(
            code: "Customer.InvalidUsername",
            description: $"Customer username must be at least {ValueObjects.Credentials.MinLoginLength} characters long and at most {ValueObjects.Credentials.MaxLoginLength} characters long.");

            public static Error InvalidEmail => Error.Validation(
            code: "Customer.InvalidEmail",
            description: "Customer email is not valid.");

            public static Error WeakPassword => Error.Validation(
            code: "Customer.WeakPassword",
            description: "Customer password is not strong enough.");

            public static Error InvalidFirstName => Error.Validation(
            code: "Customer.InvalidFirstName",
            description: "Customer first name is not valid.");

            public static Error InvalidLastName => Error.Validation(
            code: "Customer.InvalidLastName",
            description: "Customer last name is not valid.");

            public static Error NotSupportedCountry => Error.Validation(
            code: "Customer.NotSupportedCountry",
            description: $"Sam Car Sharing service is not working in this country or temporarily suspended. List of supported countries: {string.Join(", ", Entities.Customer.AllowedCountries)}");

            public static Error InvalidCity => Error.Validation(
            code: "Customer.InvalidCity",
            description: $"Customer city must be at least {Entities.Customer.MinCityLength} characters long and at most {Entities.Customer.MaxCityLength} characters long.");

            public static Error InvalidAddress => Error.Validation(
            code: "Customer.InvalidAddress",
            description: $"Customer address must be at least {Entities.Customer.MinAddressLength} characters long and at most {Entities.Customer.MaxAddressLength} characters long.");

            public static Error InvalidPhoneNumber => Error.Validation(
            code: "Customer.InvalidPhoneNumber",
            description: "Customer phone number is not valid.");

            public static Error InvalidDriverLicenseIdentifier => Error.Validation(
            code: "Customer.InvalidDriverLicenseIdentifier",
            description: $"Customer driver license identifier must be at least {Entities.Customer.MinDriverLicenseIdentifierLength} characters long and at most {Entities.Customer.MaxDriverLicenseIdentifierLength} characters long.");

            public static Error InvalidPostcode => Error.Validation(
            code: "Customer.InvalidPostcode",
            description: $"Customer postcode must be at least {Entities.Customer.MinPostcodeLength} characters long and at most {Entities.Customer.MaxPostcodeLength} characters long.");
        }
    }
}
