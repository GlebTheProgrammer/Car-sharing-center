using ErrorOr;

namespace CarSharingApp.Application.ServiceErrors
{
    public static partial class ApplicationErrors
    {
        public static class Vehicle
        {
            public static Error NotFound => Error.NotFound(
                code: "Vehicle.NotFound",
                description: "Vehicle not found");
        }
    }
}
