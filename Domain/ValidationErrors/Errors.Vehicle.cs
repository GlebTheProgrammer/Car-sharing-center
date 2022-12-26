using ErrorOr;

namespace CarSharingApp.Domain.ValidationErrors
{
    public static partial class DomainErrors
    {
        public static class Vehicle
        {
            public static Error InvalidName => Error.Validation(
            code: "Vehicle.InvalidName",
            description: $"Vehicle name must be at least {Entities.Vehicle.MinNameLength} characters long and at most {Entities.Vehicle.MaxNameLength} characters long.");

            public static Error InvalidBriefDescription => Error.Validation(
            code: "Vehicle.InvalidBriefDescription",
            description: $"Vehicle brief description must be at least {Entities.Vehicle.MinBriefDescriptionLength} characters long and at most {Entities.Vehicle.MaxBriefDescriptionLength} characters long.");

            public static Error InvalidDescription => Error.Validation(
            code: "Vehicle.InvalidDescription",
            description: $"Vehicle description must be at least {Entities.Vehicle.MinDescriptionLength} characters long and at most {Entities.Vehicle.MaxDescriptionLength} characters long.");

            public static Error InvalidLocation => Error.Validation(
            code: "Vehicle.InvalidCoordinated",
            description: $"Vehicle location variables must be fractional numbers, fractional part of which is separated by a dot.");

            public static Error InvalidAddress => Error.Validation(
            code: "Vehicle.InvalidAddress",
            description: $"Vehicle address must be at least {ValueObjects.Location.MinAddressLength} characters long and at most {ValueObjects.Location.MaxAddressLength} characters long.");

            public static Error InvalidRentalPriceDifference => Error.Validation(
            code: "Vehicle.InvalidRentalPriceDifference",
            description: $"Vehicle hourly rental price must be less than daily rental price.");

            public static Error InvalidRentalPrice => Error.Validation(
            code: "Vehicle.InvalidRentalPrice",
            description: $"Vehicle rental price must be somewhere between {ValueObjects.Tariff.MinPrice}$ and {ValueObjects.Tariff.MaxPrice}$.");

            public static Error InvalidProductionYear => Error.Validation(
            code: "Vehicle.InvalidProductionYear",
            description: $"Vehicle production year must be somewhere between {ValueObjects.Specifications.MinProductionYear} and {ValueObjects.Specifications.MaxProductionYear}.");

            public static Error InvalidMaxPossibleSpeed => Error.Validation(
            code: "Vehicle.InvalidMaxPossibleSpeed",
            description: $"Vehicle max possible speed must be somewhere between {ValueObjects.Specifications.MinPossibleSpeed} and {ValueObjects.Specifications.MaxPossibleSpeed}.");

            public static Error InvalidVIN => Error.Validation(
            code: "Vehicle.InvalidVIN",
            description: $"Provided VIN doesn't match international format.");
        }
    }
}
