namespace CarSharingApp.Domain.Exceptions
{
    public class InvalidVehicleMaxSpeedException : Exception
    {
        public InvalidVehicleMaxSpeedException()
            : base("Invalid vehicle speed.")
        {
        }
    }
}
