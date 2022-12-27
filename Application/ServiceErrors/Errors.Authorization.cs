using ErrorOr;

namespace CarSharingApp.Application.ServiceErrors
{
    public static partial class ApplicationErrors
    {
        public static class Authorization
        {
            public static Error Forbidden => Error.Custom(
            403,
            code: "Authorization.Forbidden",
            description: "Customer with provided login and password was not found.");
        }
    }
}
