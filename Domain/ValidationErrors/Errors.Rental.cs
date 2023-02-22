using ErrorOr;

namespace CarSharingApp.Domain.ValidationErrors
{
    public static partial class DomainErrors
    {
        public static class Rental
        {
            public static Error InvalidRentalStartsEndsDateTime => Error.Validation(
            code: "Rental.InvalidRentalStartsEndsDateTime.RentalStartsDateTime",
            description: "Rental starts DateTime can not be greater then ends DateTime.");
        }
    }
}
