using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class Engine : Enumeration<Engine>
    {
        public static readonly Engine Thermal = new(1, "Thermal");
        public static readonly Engine Electrical = new(2, "Electrical");
        public static readonly Engine Physical = new(3, "Physical");

        private Engine(int value, string name) : base(value, name)
        {
        }
    }
}
