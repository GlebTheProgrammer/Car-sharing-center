namespace CarSharingApp.Domain.Exceptions
{
    public class UnsupportedVehicleColourException : Exception
    {
        public UnsupportedVehicleColourException(string code)
            :base($"Colour with code {code} is not supported.")
        {
        }
    }
}
