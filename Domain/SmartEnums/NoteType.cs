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

        public static readonly NoteType RentedNewVehicle = new(30, "Vehicle rented successfully.");
        public static readonly NoteType OwnerVehicleRented = new(31, "Someone has rented your vehicle.");
        public static readonly NoteType CustomerHasFinishedExistingRental = new(32, "Rental finished successfully.");
        public static readonly NoteType SystemHasFinishedExistingRental = new(33, "Rental has over.");
        public static readonly NoteType OwnerVehicleRentalFinished = new(34, "Vehicle's rental is over.");

        private NoteType(int value, string name) : base(value, name)
        {
        }
    }
}
