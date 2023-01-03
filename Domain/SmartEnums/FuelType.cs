using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class FuelType : Enumeration<FuelType>
    {
        public static readonly FuelType Electric = new(1, "Electric");
        public static readonly FuelType Gasoline = new(2, "Gasoline");
        public static readonly FuelType Disel = new(3, "Disel");
        public static readonly FuelType BioDisel = new(4, "Bio Disel");
        public static readonly FuelType Ethanol = new(5, "Ethanol");

        private FuelType(int value, string name) : base(value, name)
        {
        }
    }
}
