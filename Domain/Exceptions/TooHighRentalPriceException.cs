namespace CarSharingApp.Domain.Exceptions
{
    public class TooHighRentalPriceException : Exception
    {
        public TooHighRentalPriceException(int maxPrice)
            :base($"Price value can't be higher than {maxPrice}.")
        {
        }
    }
}
