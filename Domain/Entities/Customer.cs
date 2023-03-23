using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValueObjects;
using ErrorOr;
using System.Text.RegularExpressions;
using CarSharingApp.Domain.ValidationErrors;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Customer : Entity
    {
        public static readonly Regex FirstLastNameRegex = new Regex("^[^±!@£$%^&*_+§¡€#¢§¶•ªº«\\/<>?:;|=.,]{1,20}$");
        public static readonly Regex PhoneNumberRegex = new Regex("\\(?\\+[0-9]{1,3}\\)? ?-?[0-9]{1,3} ?-?[0-9]{3,5} ?-?[0-9]{4}( ?-?[0-9]{3})? ?(\\w{1,10}\\s?\\d{1,6})?");
        public const int MinDriverLicenseIdentifierLength = 5;
        public const int MaxDriverLicenseIdentifierLength = 30;

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Address Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string DriverLicenseIdentifier { get; private set; }
        public Profile Profile { get; private set; }
        public Statistics Statistics { get; private set; }
        public bool HasAcceptedNewsSharing { get; private set; }
        public Credentials Credentials { get; private set; }

        private Customer(
            Guid id,
            string firstName,
            string lastName,
            Address address,
            string phoneNumer,
            string driverLicenseIdentifier,
            Profile profile,
            Statistics statistics,
            bool hasAcceptedNewsSharing,
            Credentials credentials)
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PhoneNumber = phoneNumer;
            DriverLicenseIdentifier = driverLicenseIdentifier;
            Profile = profile;
            Statistics = statistics;
            HasAcceptedNewsSharing = hasAcceptedNewsSharing;
            Credentials = credentials;
        }

        public static ErrorOr<Customer> Create(
            string firstName,
            string lastName,
            string streetAddress,
            string aptSuiteEtc,
            string city,
            string country,
            string zipPostCode,
            string phoneNumber,
            string driverLicenseIdentifier,
            bool hasAcceptedNewsSharing,
            string login,
            string email,
            string password,
            Guid? id = null,
            string? profileDescription = null,
            string? profileImage = null,
            int? vehiclesOrdered = null,
            int? vehiclesShared = null,
            bool requirePasswordEncryption = true)
        {
            List<Error> errors = new();

            if (!FirstLastNameRegex.IsMatch(firstName))
            {
                errors.Add(DomainErrors.Customer.InvalidFirstName);
            }
            if (!FirstLastNameRegex.IsMatch(lastName))
            {
                errors.Add(DomainErrors.Customer.InvalidLastName);
            }
            if (!PhoneNumberRegex.IsMatch(phoneNumber))
            {
                errors.Add(DomainErrors.Customer.InvalidPhoneNumber);
            }
            if (driverLicenseIdentifier.Length is > MaxDriverLicenseIdentifierLength or < MinDriverLicenseIdentifierLength)
            {
                errors.Add(DomainErrors.Customer.InvalidDriverLicenseIdentifier);
            }

            ErrorOr<Address> addressCreateRequest = Address.Create(streetAddress, aptSuiteEtc, city, country, zipPostCode);
            if (addressCreateRequest.IsError)
            {
                errors.AddRange(addressCreateRequest.Errors);
            }

            ErrorOr<Credentials> credentialsCreateRequest = requirePasswordEncryption 
                ? Credentials.Create(login, email, password) 
                : Credentials.CreateForUpdate(login, email, password);

            if (credentialsCreateRequest.IsError)
            {
                errors.AddRange(credentialsCreateRequest.Errors);
            }

            ErrorOr<Profile> profileCreateRequest = Profile.Create(profileDescription, profileImage);
            if (profileCreateRequest.IsError)
            {
                errors.AddRange(profileCreateRequest.Errors);
            }

            ErrorOr<Statistics> statisticsCreateRequest = Statistics.Create(vehiclesOrdered, vehiclesShared);
            if (statisticsCreateRequest.IsError)
            {
                errors.AddRange(statisticsCreateRequest.Errors);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Customer(
                id ?? Guid.NewGuid(),
                firstName,
                lastName,
                addressCreateRequest.Value,
                phoneNumber,
                driverLicenseIdentifier,
                profileCreateRequest.Value,
                statisticsCreateRequest.Value,
                hasAcceptedNewsSharing,
                credentialsCreateRequest.Value);
        }
    }
}
