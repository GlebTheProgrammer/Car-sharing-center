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

            public static Error EmailHasAlreadyExist => Error.Conflict(
            code: "Customer.EmailHasAlreadyExists",
            description: "Customer with the same email has already been registered. Please sign in or choose another one.");

            public static Error UsernameHasAlreadyExist => Error.Conflict(
            code: "Customer.UsernameHasAlreadyExists",
            description: "This username has already been taken by another customer. Please choose another one.");

            public static Error UsernameAndEmailHaveAlreadyExist => Error.Conflict(
            code: "Customer.UsernameAndEmailHaveAlreadyExist",
            description: "Customer with the same username and email has already been registered. Please sign in or choose another one.");
        }
    }
}
