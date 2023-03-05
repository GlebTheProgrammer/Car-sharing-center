using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.SmartEnums;
using ErrorOr;
using CarSharingApp.Domain.ValidationErrors;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Address : ValueObject
    {
        public const int MinStreetAddressLength = 5;
        public const int MaxStreetAddressLength = 25;
        public const int MinAptSuitEtcLength = 1;
        public const int MaxAptSuitEtcLength = 25;
        public const int MinCityLength = 5;
        public const int MaxCityLength = 25;
        public const int MinZipPostCodeLength = 5;
        public const int MaxZipPostCodeLength = 15;

        public string StreetAddress { get; private set; }
        public string AptSuiteEtc { get; private set; }
        public string City { get; private set; }
        public Country Country { get; private set; }
        public string ZipPostCode { get; private set; }

        private Address(
            string streetAddress, 
            string aptSuitEtc, 
            string city, 
            Country country, 
            string zipPostCode)
        {
            StreetAddress = streetAddress;
            AptSuiteEtc = aptSuitEtc;
            City = city;
            Country = country;
            ZipPostCode = zipPostCode;
        }

        public static ErrorOr<Address> Create(
            string streetAddress,
            string aptSuiteEtc,
            string city,
            string country,
            string zipPostCode)
        {
            List<Error> errors = new();

            if (streetAddress.Length is > MaxStreetAddressLength or < MinStreetAddressLength)
            {
                errors.Add(DomainErrors.Customer.InvalidStreetAddress);
            }
            if (aptSuiteEtc.Length is > MaxAptSuitEtcLength or < MinAptSuitEtcLength)
            {
                errors.Add(DomainErrors.Customer.InvalidAptSuiteEtc);
            }
            if (city.Length is > MaxCityLength or < MinCityLength)
            {
                errors.Add(DomainErrors.Customer.InvalidCity);
            }
            if (Country.FromName(country) is null)
            {
                errors.Add(DomainErrors.Customer.NotSupportedCountry);
            }
            if (zipPostCode.Length is > MaxZipPostCodeLength or < MinZipPostCodeLength)
            {
                errors.Add(DomainErrors.Customer.InvalidZipPostCode);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Address(
                streetAddress, 
                aptSuiteEtc, 
                city, 
                Country.FromName(country) ?? throw new ArgumentNullException(nameof(country)), 
                zipPostCode);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return StreetAddress;
            yield return AptSuiteEtc;
            yield return City;
            yield return Country.ToString();
            yield return ZipPostCode;
        }
    }
}
