using CarSharingApp.Domain.SmartEnums;
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
            description: "Vehicle latitude and longitude variables (location) must be fractional numbers, fractional part of which is separated by a dot.");

            public static Error InvalidAddress => Error.Validation(
            code: "Vehicle.InvalidAddress",
            description: $"Vehicle address must be at least {ValueObjects.Location.MinAddressLength} characters long and at most {ValueObjects.Location.MaxAddressLength} characters long.");

            public static Error InvalidRentalPriceDifference => Error.Validation(
            code: "Vehicle.InvalidRentalPriceDifference",
            description: "Vehicle hourly rental price must be less than daily rental price.");

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
            description: "Provided VIN doesn't match international format.");

            public static Error InvalidInteriorColour => Error.Validation(
            code: "Vehicle.InvalidInteriorColour",
            description: $"Vehicle interior colour is not valid. List of possible colours: {string.Join(", ", Colour.GetPossibleEnumerations())}");

            public static Error InvalidExteriorColour => Error.Validation(
            code: "Vehicle.InvalidExteriorColour",
            description: $"Vehicle exterior colour is not valid. List of possible colours: {string.Join(", ", Colour.GetPossibleEnumerations())}");

            public static Error InvalidDrivetrain => Error.Validation(
            code: "Vehicle.InvalidDrivetrain",
            description: $"Vehicle drivetrain is not valid. List of possible drivetrains: {string.Join(", ", Drivetrain.GetPossibleEnumerations())}");

            public static Error InvalidFuelType => Error.Validation(
            code: "Vehicle.InvalidFuelType",
            description: $"Vehicle fuel type is not valid. List of possible fuel types: {string.Join(", ", FuelType.GetPossibleEnumerations())}");

            public static Error InvalidEngine => Error.Validation(
            code: "Vehicle.InvalidEngine",
            description: $"Vehicle engine is not valid. List of possible engines: {string.Join(", ", Engine.GetPossibleEnumerations())}");

            public static Error InvalidTransmission => Error.Validation(
            code: "Vehicle.InvalidTransmission",
            description: $"Vehicle transmission is not valid. List of possible transmissions: {string.Join(", ", Transmission.GetPossibleEnumerations())}");

        }
    }
}
