namespace CarSharingApp.Domain.Exceptions
{
    public class InvalidVehicleLatitudeLongitudeException : Exception
    {
        public InvalidVehicleLatitudeLongitudeException()
            : base("Wrong latitude and longitude were set.")
        {
        }
    }
}
