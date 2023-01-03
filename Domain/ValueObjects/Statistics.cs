using CarSharingApp.Domain.Primitives;
using ErrorOr;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Statistics : ValueObject
    {
        public const int DefaultVehiclesOrderedCount = 0;
        public const int DefaultVehiclesSharedCount = 0;

        public int VehiclesOrdered { get; private set; }
        public int VehiclesShared { get; private set; }

        private Statistics(
            int vehiclesOrdered,
            int vehiclesShared)
        {
            VehiclesOrdered = vehiclesOrdered;
            VehiclesShared = vehiclesShared;
        }

        public static ErrorOr<Statistics> Create(
            int? vehiclesOrdered,
            int? vehiclesShared)
        {
            return new Statistics(
                vehiclesOrdered ?? DefaultVehiclesOrderedCount,
                vehiclesShared ?? DefaultVehiclesSharedCount);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return VehiclesOrdered;
            yield return VehiclesShared;
        }
    }
}
