using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class Drivetrain : Enumeration<Drivetrain>
    {
        public static readonly Drivetrain AllWheelDrive = new(1, "All wheel drive");
        public static readonly Drivetrain FourWheelDrive = new(2, "Four wheel drive");
        public static readonly Drivetrain FrontWheelDrive = new(3, "Front wheel drive");
        public static readonly Drivetrain RealWheelDrive = new(4, "Real wheel drive");

        private Drivetrain(int value, string name) 
            : base(value, name)
        {
        }
    }
}
