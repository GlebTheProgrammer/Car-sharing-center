namespace CarSharingApp.Domain.Exceptions
{
    public class NegativeRentalPriceException : Exception
    {
        public NegativeRentalPriceException()
            : base ("Price value can't be as low.")
        {
        }
    }
}
