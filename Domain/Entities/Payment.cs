using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Payment : Entity
    {
        public DateTime Date { get; private set; }
        public decimal Amount { get; private set; }

        public Guid RentalId { get; private set; } // 1:1
        public Rental? Rental { get; private set; }

        public Payment(Guid rentalId,
            DateTime date, 
            decimal amount)
        {
            RentalId = rentalId;
            Date = date;
            Amount = amount;
        }
    }
}
