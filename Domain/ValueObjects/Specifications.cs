using CarSharingApp.Domain.Enums;
using CarSharingApp.Domain.Primitives;
using ErrorOr;
using System.Text.RegularExpressions;
using CarSharingApp.Domain.ValidationErrors;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Specifications : ValueObject
    {
        public static readonly int MaxProductionYear = DateTime.Now.Year;
        public const int MinProductionYear = 2000;
        public const int MaxPossibleSpeed = 1000;
        public const int MinPossibleSpeed = 200;
        public static readonly Regex vinRegex = new Regex("^[A-HJ-NPR-Za-hj-npr-z\\d]{8}[\\dX][A-HJ-NPR-Za-hj-npr-z\\d]{2}\\d{6}$");

        public int ProductionYear { get; private set; }
        public int MaxSpeedKph { get; private set; }
        public string ExteriorColor { get; private set; }
        public string InteriorColor { get; private set; }
        public Drivetrain Drivetrain { get; private set; }
        public FuelType FuelType { get; private set; }
        public Transmission Transmission { get; private set; }
        public Engine Engine { get; private set; }
        public string VIN { get; private set; }

        private Specifications(
            int productionYear,
            int maxSpeedKph,
            string exteriorColor,
            string interiorColor,
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
            Drivetrain drivetrain,
            FuelType fuelType,
            Transmission transmission,
            Engine engine,
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

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Specifications(
                productionYear, 
                maxSpeedKph, 
                exteriorColor, 
                interiorColor, 
                drivetrain, 
                fuelType, 
                transmission, 
                engine, 
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
