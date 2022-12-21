namespace CarSharingApp.Domain.Exceptions
{
    public class InvalidVehicleVINException : Exception
    {
        public InvalidVehicleVINException()
            :base("Vehicle VIN is invalid.")
        {
        }
    }
}
