namespace CarSharingApp.Domain.Exceptions
{
    public class VehicleLocationNullArgumentsException : Exception
    {
        public VehicleLocationNullArgumentsException()
            :base("Vehicle location field was null.")
        {
        }
    }
}
