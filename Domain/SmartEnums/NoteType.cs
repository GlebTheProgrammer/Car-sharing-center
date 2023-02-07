using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class NoteType : Enumeration<NoteType>
    {
        public static readonly NoteType RegisteredNewCustomer = new(10, "Welcome.");
        public static readonly NoteType UpdatedCustomerCredentials = new(11, "Credentials updated.");
        public static readonly NoteType UpdatedCustomerInfo = new(12, "Account Info updated.");
        public static readonly NoteType UpdatedCustomerPassword = new(13, "Password updated.");

        public static readonly NoteType AddedNewVehicle = new(20, "Vehicle added.");
        public static readonly NoteType DeletedExistingVehicle = new(21, "Vehicle deleted.");
        public static readonly NoteType UpdatedExistingVehicle = new(22, "Vehicle Info updated.");

        private NoteType(int value, string name) : base(value, name)
        {
        }
    }
}
