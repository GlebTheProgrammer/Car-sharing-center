namespace CarSharingApp.Domain.Exceptions
{
    public class InvalidVehicleProductionYearException : Exception
    {
        public InvalidVehicleProductionYearException()
            :base($"Invalid vehicle production year.")
        {
        }
    }
}
