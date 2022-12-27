using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Rental : Entity
    {
        public Guid RentedCustomerId { get; private set; } // 1:1
        public Customer? RentedCustomer { get; private set; }
        public Guid VehicleOwnerId { get; private set; } // 1:1
        public Customer? VehicleOwner { get; private set; }
        public Guid VehicleId { get; private set; } // 1:1
        public Vehicle? Vehicle { get; private set; }
        public Guid PaymentId { get; private set; } // 1:1
        public Payment? Payment { get; private set; }

        public DateTime RentalDate { get; private set; }
        public int RentalTimeInMinutes { get; private set; }
        public DateTime ReturnDate { get; private set; }
        public bool IsActive { get; private set; }

        public Rental(Guid id,
            Guid rentedCustomerId, 
            Guid vehicleOwnerId,
            Guid vehicleId,
            Guid paymentId,
            DateTime rentalDate, 
            int rentalTimeInMinutes, 
            DateTime returnDate, 
            bool isActive)
            : base(id)
        {
            RentedCustomerId = rentedCustomerId;
            VehicleOwnerId = vehicleOwnerId;
            VehicleId = vehicleId;
            PaymentId = paymentId;
            RentalDate = rentalDate;
            RentalTimeInMinutes = rentalTimeInMinutes;
            ReturnDate = returnDate;
            IsActive = isActive;
        }
    }
}
