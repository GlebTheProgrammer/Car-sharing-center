using CarSharingApp.Domain.Primitives;
using System.Text.RegularExpressions;
using ErrorOr;
using CarSharingApp.Domain.ValidationErrors;
using CarSharingApp.Domain.SmartEnums;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Location : ValueObject
    {
        public const int MinStreetAddressLength = 5;
        public const int MaxStreetAddressLength = 25;
        public const int MinAptSuiteEtcLength = 3;
        public const int MaxAptSuiteEtcLength = 25;
        public const int MinCityLength = 5;
        public const int MaxCityLength = 25;
        public static readonly Regex LatitudeLongitudeRegex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public string StreetAddress { get; private set; }
        public string AptSuiteEtc { get; private set; }
        public string City { get; private set; }
        public Country Country { get; private set; }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }

        private Location(
            string streetAddress, 
            string aptSuiteEtc, 
            string city,
            Country country,
            string latitude,
            string longitude)
        {
            StreetAddress = streetAddress;
            AptSuiteEtc = aptSuiteEtc;
            City = city;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
        }

        public static ErrorOr<Location> Create(
            string streetAddress, 
            string aptSuiteEtc,
            string city,
            string country,
            string latitude,
            string longitude)
        {
            List<Error> errors = new();

            if (streetAddress.Length is > MaxStreetAddressLength or < MinStreetAddressLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidStreetAddress);
            }
            if (aptSuiteEtc.Length is > MaxAptSuiteEtcLength or < MinAptSuiteEtcLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidAptSuiteEtc);
            }
            if (city.Length is > MaxCityLength or < MinCityLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidCity);
            }
            if (Country.FromName(country) is null)
            {
                errors.Add(DomainErrors.Vehicle.NotSupportedCountry);
            }
            if (!LatitudeLongitudeRegex.IsMatch(latitude) || !LatitudeLongitudeRegex.IsMatch(longitude))
            {
                errors.Add(DomainErrors.Vehicle.InvalidLatitudeLongitude);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Location(
                streetAddress,
                aptSuiteEtc,
                city,
                Country.FromName(country) ?? throw new ArgumentNullException(nameof(country)),
                latitude,
                longitude);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return StreetAddress;
            yield return AptSuiteEtc;
            yield return City;
            yield return Country.Name;
            yield return Latitude;
            yield return Longitude;
        }
    }
}
