using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;
using System.Text.RegularExpressions;
using CarSharingApp.Domain.ValidationErrors;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Customer : Entity
    {
        public static readonly string[] AllowedCountries = new[] { "Germany", "Latvia", "USA", "Netherlands", "Poland", "Cyprus", "Belarus", "Ukraine", "Lithuania" };
        public const int MinCityLength = 5;
        public const int MaxCityLength = 25;
        public const int MinAddressLength = 5;
        public const int MaxAddressLength = 50;
        public static readonly Regex PhoneNumberRegex = new Regex("^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$");
        public const int MinDriverLicenseIdentifierLength = 5;
        public const int MaxDriverLicenseIdentifierLength = 30;
        public const int MinPostcodeLength = 5;
        public const int MaxPostcodeLength = 15;
        public const string DefaultProfileDescription = "No description yet";
        public const string DefaultProfileImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTdmrjoiXGVFEcd1cX9Arb1itXTr2u8EKNpw&usqp=CAU";


        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string DriverLicenseIdentifier { get; private set; }
        public string ProfileDescription { get; private set; }
        public string ProfileImage { get; private set; }
        public string Postcode { get; private set; }
        public int VehiclesOrdered { get; private set; }
        public int VehiclesShared { get; private set; }
        public bool HasAcceptedNewsSharing { get; private set; }
        public bool IsOnline { get; private set; }
        public Credentials Credentials { get; private set; }

        private Customer(
            Guid id,
            string firstName,
            string lastName,
            string country,
            string city,
            string address,
            string phoneNumer,
            string driverLicenseIdentifier,
            string profileDecription,
            string profileImage,
            string postcode,
            int vehiclesOrdered,
            int vehiclesShared,
            bool hasAcceptedNewsSharing,
            bool isOnline,
            Credentials credentials)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            City = city;
            Address = address;
            PhoneNumber = phoneNumer;
            DriverLicenseIdentifier = driverLicenseIdentifier;
            ProfileDescription = profileDecription;
            ProfileImage = profileImage;
            Postcode = postcode;
            VehiclesOrdered = vehiclesOrdered;
            VehiclesShared = vehiclesShared;
            HasAcceptedNewsSharing = hasAcceptedNewsSharing;
            IsOnline = isOnline;
            Credentials = credentials;
        }

        public static ErrorOr<Customer> Create(
            string firstName,
            string lastName,
            string country,
            string city,
            string address,
            string phoneNumber,
            string driverLicenseIdentifier,
            string postcode,
            bool hasAcceptedNewsSharing,
            string login,
            string email,
            string password,
            Guid? id = null,
            string? profileDescription = null,
            string? profileImage = null,
            int? vehiclesOrdered = null,
            int? vehiclesShared = null,
            bool? isOnline = null)
        {
            List<Error> errors = new();

            if (firstName.Length is 0)
            {
                errors.Add(DomainErrors.Customer.InvalidFirstName);
            }
            if (lastName.Length is 0)
            {
                errors.Add(DomainErrors.Customer.InvalidLastName);
            }
            if (!AllowedCountries.Contains(country))
            {
                errors.Add(DomainErrors.Customer.NotSupportedCountry);
            }
            if (city.Length is > MaxCityLength or < MinCityLength)
            {
                errors.Add(DomainErrors.Customer.InvalidCity);
            }
            if (address.Length is > MaxAddressLength or < MinAddressLength)
            {
                errors.Add(DomainErrors.Customer.InvalidAddress);
            }
            if (!PhoneNumberRegex.IsMatch(phoneNumber))
            {
                errors.Add(DomainErrors.Customer.InvalidPhoneNumber);
            }
            if (driverLicenseIdentifier.Length is > MaxDriverLicenseIdentifierLength or < MinDriverLicenseIdentifierLength)
            {
                errors.Add(DomainErrors.Customer.InvalidDriverLicenseIdentifier);
            }
            if (postcode.Length is > MaxPostcodeLength or < MinPostcodeLength)
            {
                errors.Add(DomainErrors.Customer.InvalidPostcode);
            }

            ErrorOr<Credentials> credentialsCreateRequest = Credentials.Create(login, email, password);
            if (credentialsCreateRequest.IsError)
            {
                errors.AddRange(credentialsCreateRequest.Errors);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Customer(
                id ?? Guid.NewGuid(),
                firstName,
                lastName,
                country,
                city, 
                address,
                phoneNumber,
                driverLicenseIdentifier,
                profileDescription ?? DefaultProfileDescription,
                profileImage ?? DefaultProfileImage,
                postcode,
                vehiclesOrdered ?? 0,
                vehiclesShared ?? 0,
                hasAcceptedNewsSharing,
                isOnline ?? false,
                credentialsCreateRequest.Value);
        }
    }
}
