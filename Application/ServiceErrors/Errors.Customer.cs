using ErrorOr;

namespace CarSharingApp.Application.ServiceErrors
{
    public static partial class ApplicationErrors
    {
        public static class Customer
        {
            public static Error NotFound => Error.NotFound(
                code: "Customer.NotFound",
                description: "Customer not found");
        }
    }
}
