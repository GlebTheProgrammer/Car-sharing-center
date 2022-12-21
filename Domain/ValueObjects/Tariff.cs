using CarSharingApp.Domain.Exceptions;
using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Tariff : ValueObject
    {
        public const int MaxPrice = 9999;

        public decimal HourlyRentalPrice { get; private set; }
        public decimal DailyRentalPrice { get; private set; }

        public Tariff(decimal hourlyRentalPrice, decimal dailyRentalPrice)
        {
            if (hourlyRentalPrice <= 0 || dailyRentalPrice <= 0)
            {
                throw new NegativeRentalPriceException();
            }

            if (hourlyRentalPrice > dailyRentalPrice)
            {
                throw new HourlyRentalPriceLargerThanDailyException();
            }

            if (hourlyRentalPrice > MaxPrice && dailyRentalPrice > MaxPrice)
            {
                throw new TooHighRentalPriceException(MaxPrice);
            }

            HourlyRentalPrice = hourlyRentalPrice;
            DailyRentalPrice = dailyRentalPrice;
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return HourlyRentalPrice;
            yield return DailyRentalPrice;
        }
    }
}
