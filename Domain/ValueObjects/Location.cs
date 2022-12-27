using CarSharingApp.Domain.Primitives;
using System.Text.RegularExpressions;
using ErrorOr;
using CarSharingApp.Domain.ValidationErrors;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Location : ValueObject
    {
        public const int MinAddressLength = 15;
        public const int MaxAddressLength = 30;
        public static readonly Regex LocationRegex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public string Address { get; private set; }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }

        private Location(string address, string latitude, string longitude)
        {
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }

        public static ErrorOr<Location> Create(string address, string latitude, string longitude)
        {
            List<Error> errors = new();

            if (!LocationRegex.IsMatch(latitude) || !LocationRegex.IsMatch(longitude))
            {
                errors.Add(DomainErrors.Vehicle.InvalidLocation);
            }
            if (address.Length is < MinAddressLength or > MaxAddressLength)
            {
                errors.Add(DomainErrors.Vehicle.InvalidAddress);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Location(address, latitude, longitude);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Address;
            yield return Latitude;
            yield return Longitude;
        }
    }
}
