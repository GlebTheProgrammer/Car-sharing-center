namespace CarSharingApp.Domain.Exceptions
{
    public class HourlyRentalPriceLargerThanDailyException : Exception
    {
        public HourlyRentalPriceLargerThanDailyException()
            :base("Daily price must be larger than hourly.")
        {
        }
    }
}
