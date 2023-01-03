using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class Transmission : Enumeration<Transmission>
    {
        public static readonly Transmission Automatic = new(1, "Automatic");
        public static readonly Transmission Manual = new(2, "Manual");
        public static readonly Transmission AutomatedManual = new(3, "Automated Manual");
        public static readonly Transmission ContinuouslyVariable = new(4, "Continuously Variable");

        private Transmission(int value, string name) : base(value, name)
        {
        }
    }
}
