using ErrorOr;

namespace CarSharingApp.Application.ServiceErrors
{
    public static partial class ApplicationErrors
    {
        public static class Payment
        {
            public static Error NotFound => Error.NotFound(
                code: "Payment.NotFound",
                description: "Payment was not found.");
        }
    }
}
