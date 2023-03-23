using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValidationErrors;
using ErrorOr;
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

        public Payment? Payment { get; private set; } // 1:1 EF Core

        [Required]
        public DateTime RentalStartsDateTime { get; private set; }
        [Required]
        public DateTime RentalEndsDateTime { get; private set; }
        [Required]
        public bool IsActive { get; private set; }

        private Rental(Guid id,
            Guid rentedCustomerId, 
            Guid vehicleOwnerId,
            Guid vehicleId,
            DateTime rentalStartsDateTime,
            DateTime rentalEndsDateTime)
            : base(id)
        {
            RentedCustomerId = rentedCustomerId;
            VehicleOwnerId = vehicleOwnerId;
            VehicleId = vehicleId;
            RentalStartsDateTime = rentalStartsDateTime;
            RentalEndsDateTime = rentalEndsDateTime;
            IsActive = true;
        }

        public static ErrorOr<Rental> Create(
            Guid rentedCustomerId,
            Guid vehicleOwnerId,
            Guid vehicleId,
            DateTime rentalStartsDateTime,
            DateTime rentalEndsDateTime,
            Guid? id = null)
        {
            List<Error> errors = new();

            if (rentalStartsDateTime >= rentalEndsDateTime)
            {
                errors.Add(DomainErrors.Rental.InvalidRentalStartsEndsDateTime);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Rental(
                id ?? Guid.NewGuid(),
                rentedCustomerId,
                vehicleOwnerId,
                vehicleId,
                rentalStartsDateTime,
                rentalEndsDateTime);
        }

        public static Rental Finish(Rental rentalToFinish)
        {
            rentalToFinish.IsActive = false;

            return rentalToFinish;
        }
    }
}
