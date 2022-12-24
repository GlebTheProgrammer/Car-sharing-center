using CarSharingApp.Domain.Primitives;
using System.Text.RegularExpressions;
using CarSharingApp.Domain.Exceptions;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Location : ValueObject
    {
        public readonly Regex locationRegex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

        public string Address { get; private set; }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }

        public Location(string address, string latitude, string longitude)
        {
            if (address == null || latitude == null || longitude == null)
            {
                throw new VehicleLocationNullArgumentsException();
            }

            if (!locationRegex.IsMatch(latitude) || !locationRegex.IsMatch(longitude))
            {
                throw new InvalidVehicleLatitudeLongitudeException();
            }

            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Address;
            yield return Latitude;
            yield return Longitude;
        }
    }
}
