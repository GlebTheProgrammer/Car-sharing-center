using CarSharingApp.Domain.Enums;
using CarSharingApp.Domain.Exceptions;
using CarSharingApp.Domain.Primitives;
using System.Text.RegularExpressions;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Specifications : ValueObject
    {
        public readonly int maxProductionYear = DateTime.Now.Year;
        public const int maxPossibleSpeed = 1000;
        public readonly Regex vinRegex = new Regex("^[A-HJ-NPR-Za-hj-npr-z\\d]{8}[\\dX][A-HJ-NPR-Za-hj-npr-z\\d]{2}\\d{6}$");

        public int ProductionYear { get; private set; }
        public int MaxSpeedKph { get; private set; }
        public string ExteriorColor { get; private set; }
        public string InteriorColor { get; private set; }
        public Drivetrain Drivetrain { get; private set; }
        public FuelType FuelType { get; private set; }
        public Transmission Transmission { get; private set; }
        public Engine Engine { get; private set; }
        public string VIN { get; private set; }

        public Specifications(int productionYear,
            int maxSpeedKph,
            string exteriorColor,
            string interiorColor,
            Drivetrain drivetrain,
            FuelType fuelType,
            Transmission transmission,
            Engine engine,
            string vin)
        {
            if (productionYear > maxProductionYear || productionYear <= 0) 
            {
                throw new InvalidVehicleProductionYearException();
            }

            if (maxSpeedKph > maxPossibleSpeed || maxSpeedKph <= 0)
            {
                throw new InvalidVehicleMaxSpeedException();
            }

            if (!vinRegex.IsMatch(vin))
            {
                throw new InvalidVehicleVINException();
            }

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
