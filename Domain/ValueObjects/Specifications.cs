using CarSharingApp.Domain.Primitives;
using ErrorOr;
using System.Text.RegularExpressions;
using CarSharingApp.Domain.ValidationErrors;
using CarSharingApp.Domain.SmartEnums;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Specifications : ValueObject
    {
        public static readonly int MaxProductionYear = DateTime.UtcNow.Year;
        public const int MinProductionYear = 2000;
        public const int MaxPossibleSpeed = 1000;
        public const int MinPossibleSpeed = 200;
        public static readonly Regex vinRegex = new Regex("[A-HJ-NPR-Z0-9]{17}");

        public int ProductionYear { get; private set; }
        public int MaxSpeedKph { get; private set; }
        public Colour ExteriorColor { get; private set; }
        public Colour InteriorColor { get; private set; }
        public Drivetrain Drivetrain { get; private set; }
        public FuelType FuelType { get; private set; }
        public Transmission Transmission { get; private set; }
        public Engine Engine { get; private set; }
        public string VIN { get; private set; }

        private Specifications(
            int productionYear,
            int maxSpeedKph,
            Colour exteriorColor,
            Colour interiorColor,
            Drivetrain drivetrain,
            FuelType fuelType,
            Transmission transmission,
            Engine engine,
            string vin)
        {
            ProductionYear = productionYear;
            MaxSpeedKph = maxSpeedKph;
            ExteriorColor = exteriorColor;
            InteriorColor = interiorColor;
            Drivetrain = drivetrain;
            FuelType = fuelType;
            Transmission = transmission;
            Engine = engine;
            VIN = vin;
        }

        public static ErrorOr<Specifications> Create(
            int productionYear,
            int maxSpeedKph,
            string exteriorColor,
            string interiorColor,
            string drivetrain,
            string fuelType,
            string transmission,
            string engine,
            string vin)
        {
            List<Error> errors = new();

            if (productionYear > MaxProductionYear || productionYear < MinProductionYear)
            {
                errors.Add(DomainErrors.Vehicle.InvalidProductionYear);
            }
            if (maxSpeedKph is > MaxPossibleSpeed or < MinPossibleSpeed)
            {
                errors.Add(DomainErrors.Vehicle.InvalidMaxPossibleSpeed);
            }
            if (!vinRegex.IsMatch(vin))
            {
                errors.Add(DomainErrors.Vehicle.InvalidVIN);
            }
            if (Colour.FromName(exteriorColor) is null)
            {
                errors.Add(DomainErrors.Vehicle.InvalidExteriorColour);
            }
            if (Colour.FromName(interiorColor) is null)
            {
                errors.Add(DomainErrors.Vehicle.InvalidInteriorColour);
            }
            if (Drivetrain.FromName(drivetrain) is null) 
            {
                errors.Add(DomainErrors.Vehicle.InvalidDrivetrain);
            }
            if (Engine.FromName(engine) is null)
            {
                errors.Add(DomainErrors.Vehicle.InvalidEngine);
            }
            if (FuelType.FromName(fuelType) is null)
            {
                errors.Add(DomainErrors.Vehicle.InvalidFuelType);
            }
            if (Transmission.FromName(transmission) is null)
            {
                errors.Add(DomainErrors.Vehicle.InvalidTransmission);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Specifications(
                productionYear, 
                maxSpeedKph, 
                Colour.FromName(exteriorColor) ?? throw new ArgumentNullException(nameof(exteriorColor)),
                Colour.FromName(interiorColor) ?? throw new ArgumentNullException(nameof(interiorColor)),
                Drivetrain.FromName(drivetrain) ?? throw new ArgumentNullException(nameof(drivetrain)),
                FuelType.FromName(fuelType) ?? throw new ArgumentNullException(nameof(fuelType)),
                Transmission.FromName(transmission) ?? throw new ArgumentNullException(nameof(transmission)),
                Engine.FromName(engine) ?? throw new ArgumentNullException(nameof(engine)), 
                vin);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return ProductionYear;
            yield return MaxSpeedKph;
            yield return ExteriorColor;
            yield return InteriorColor;
            yield return Drivetrain;
            yield return FuelType;
            yield return Transmission;
            yield return Engine;
            yield return VIN;
        }
    }
}
