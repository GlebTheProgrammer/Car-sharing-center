using CarSharingApp.Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Rental : Entity
    {
        [Required]
        public Guid RentedCustomerId { get; private set; }
        [Required]
        public Guid VehicleOwnerId { get; private set; }
        [Required]
        public Guid VehicleId { get; private set; }

        [Required]
        public Payment Payment { get; private set; } // 1:1 EF Core

        [Required]
        public DateTime RentalStartsDate { get; private set; }
        [Required]
        public DateTime RentalEndsDate { get; private set; }
        [Required]
        public bool IsActive { get; private set; }

        public Rental(Guid id,
            Guid rentedCustomerId, 
            Guid vehicleOwnerId,
            Guid vehicleId,
            Payment payment,
            DateTime rentalStartsDate,
            DateTime rentalEndsDate, 
            bool isActive)
            : base(id)
        {
            RentedCustomerId = rentedCustomerId;
            VehicleOwnerId = vehicleOwnerId;
            VehicleId = vehicleId;
            Payment = payment;
            RentalStartsDate = rentalStartsDate;
            RentalEndsDate = rentalEndsDate;
            IsActive = isActive;
        }
    }
}
