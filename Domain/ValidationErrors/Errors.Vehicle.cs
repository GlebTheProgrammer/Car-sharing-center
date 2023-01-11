using CarSharingApp.Domain.SmartEnums;
using ErrorOr;

namespace CarSharingApp.Domain.ValidationErrors
{
    public static partial class DomainErrors
    {
        public static class Vehicle
        {
            public static Error InvalidName => Error.Validation(
            code: "Vehicle.InvalidName.Name",
            description: $"Vehicle name must be at least {Entities.Vehicle.MinNameLength} characters long and at most {Entities.Vehicle.MaxNameLength} characters long.");

            public static Error InvalidBriefDescription => Error.Validation(
            code: "Vehicle.InvalidBriefDescription.BriefDescription",
            description: $"Vehicle brief description must be at least {Entities.Vehicle.MinBriefDescriptionLength} characters long and at most {Entities.Vehicle.MaxBriefDescriptionLength} characters long.");

            public static Error InvalidDescription => Error.Validation(
            code: "Vehicle.InvalidDescription.Description",
            description: $"Vehicle description must be at least {Entities.Vehicle.MinDescriptionLength} characters long and at most {Entities.Vehicle.MaxDescriptionLength} characters long.");

            public static Error InvalidLatitudeLongitude => Error.Validation(
            code: "Vehicle.InvalidLatitudeLongitude.Latitude",
            description: "Vehicle latitude and longitude variables (location) must be fractional numbers, fractional part of which is separated by a dot.");

            public static Error InvalidRentalPriceDifference => Error.Validation(
            code: "Vehicle.InvalidRentalPriceDifference.HourlyRentalPrice",
            description: "Vehicle hourly rental price must be less than daily rental price.");

            public static Error InvalidRentalPrice => Error.Validation(
            code: "Vehicle.InvalidRentalPrice.DailyRentalPrice",
            description: $"Vehicle rental price must be somewhere between {ValueObjects.Tariff.MinPrice}$ and {ValueObjects.Tariff.MaxPrice}$.");

            public static Error InvalidProductionYear => Error.Validation(
            code: "Vehicle.InvalidProductionYear.ProductionYear",
            description: $"Vehicle production year must be somewhere between {ValueObjects.Specifications.MinProductionYear} and {ValueObjects.Specifications.MaxProductionYear}.");

            public static Error InvalidMaxPossibleSpeed => Error.Validation(
            code: "Vehicle.InvalidMaxPossibleSpeed.MaxSpeedKph",
            description: $"Vehicle max possible speed must be somewhere between {ValueObjects.Specifications.MinPossibleSpeed} and {ValueObjects.Specifications.MaxPossibleSpeed}.");

            public static Error InvalidVIN => Error.Validation(
            code: "Vehicle.InvalidVIN.VIN",
            description: "Provided VIN doesn't match international format.");

            public static Error InvalidInteriorColour => Error.Validation(
            code: "Vehicle.InvalidInteriorColour.InteriorColor",
            description: $"Vehicle interior colour is not valid. List of possible colours: {string.Join(", ", Colour.GetPossibleEnumerations())}");

            public static Error InvalidExteriorColour => Error.Validation(
            code: "Vehicle.InvalidExteriorColour.ExteriorColor",
            description: $"Vehicle exterior colour is not valid. List of possible colours: {string.Join(", ", Colour.GetPossibleEnumerations())}");

            public static Error InvalidDrivetrain => Error.Validation(
            code: "Vehicle.InvalidDrivetrain.Drivetrain",
            description: $"Vehicle drivetrain is not valid. List of possible drivetrains: {string.Join(", ", Drivetrain.GetPossibleEnumerations())}");

            public static Error InvalidFuelType => Error.Validation(
            code: "Vehicle.InvalidFuelType.FuelType",
            description: $"Vehicle fuel type is not valid. List of possible fuel types: {string.Join(", ", FuelType.GetPossibleEnumerations())}");

            public static Error InvalidEngine => Error.Validation(
            code: "Vehicle.InvalidEngine.Engine",
            description: $"Vehicle engine is not valid. List of possible engines: {string.Join(", ", Engine.GetPossibleEnumerations())}");

            public static Error InvalidTransmission => Error.Validation(
            code: "Vehicle.InvalidTransmission.Transmission",
            description: $"Vehicle transmission is not valid. List of possible transmissions: {string.Join(", ", Transmission.GetPossibleEnumerations())}");

            public static Error InvalidCategories => Error.Validation(
            code: "Vehicle.InvalidCategories.Categories",
            description: "One of the provided categories is not allowed.");

            public static Error NotSupportedCountry => Error.Validation(
            code: "Vehicle.NotSupportedCountry.Country",
            description: $"Sam Car Sharing service is not working in this country or temporarily suspended. List of supported countries: {string.Join(", ", Country.GetPossibleEnumerations())}");

            public static Error InvalidCity => Error.Validation(
            code: "Vehicle.InvalidCity.City",
            description: $"Vehicle city must be at least {ValueObjects.Location.MinCityLength} characters long and at most {ValueObjects.Location.MaxCityLength} characters long.");

            public static Error InvalidStreetAddress => Error.Validation(
            code: "Vehicle.InvalidStreetAddress.StreetAddress",
            description: $"Vehicle street address must be at least {ValueObjects.Location.MinStreetAddressLength} characters long and at most {ValueObjects.Location.MaxStreetAddressLength} characters long.");

            public static Error InvalidAptSuiteEtc => Error.Validation(
            code: "Vehicle.InvalidAptSuiteEtc.AptSuiteEtc",
            description: $"Vehicle nearby apartment / suite number must be at least {ValueObjects.Location.MinAptSuiteEtcLength} characters long and at most {ValueObjects.Location.MaxAptSuiteEtcLength} characters long.");

        }
    }
}
