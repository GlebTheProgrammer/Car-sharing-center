using CarSharingApp.Domain.Primitives;
using ErrorOr;
using CarSharingApp.Domain.ValidationErrors;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Tariff : ValueObject
    {
        public const int MinPrice = 1;
        public const int MaxPrice = 10000;

        public decimal HourlyRentalPrice { get; private set; }
        public decimal DailyRentalPrice { get; private set; }

        private Tariff(decimal hourlyRentalPrice, decimal dailyRentalPrice)
        {
            HourlyRentalPrice = hourlyRentalPrice;
            DailyRentalPrice = dailyRentalPrice;
        }

        public static ErrorOr<Tariff> Create(decimal hourlyRentalPrice, decimal dailyRentalPrice)
        {
            List<Error> errors = new();

            if (hourlyRentalPrice >= dailyRentalPrice)
            {
                errors.Add(DomainErrors.Vehicle.InvalidRentalPriceDifference);
            }
            if (hourlyRentalPrice < MinPrice || dailyRentalPrice < MinPrice || hourlyRentalPrice > MaxPrice || dailyRentalPrice > MaxPrice)
            {
                errors.Add(DomainErrors.Vehicle.InvalidRentalPrice);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Tariff(hourlyRentalPrice, dailyRentalPrice);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return HourlyRentalPrice;
            yield return DailyRentalPrice;
        }
    }
}
