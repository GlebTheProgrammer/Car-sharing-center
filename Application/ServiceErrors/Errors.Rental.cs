using ErrorOr;

namespace CarSharingApp.Application.ServiceErrors
{
    public static partial class ApplicationErrors
    {
        public static class Rental
        {
            public static Error NotFound => Error.NotFound(
                code: "Rental.NotFound",
                description: "Rental not found");
        }
    }
}
