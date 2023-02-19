using CarSharingApp.Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Payment : Entity
    {
        [Required]
        [MaxLength(255)]
        public string StripeTransactionId { get; private set; }
        [Required]
        public DateTime PaymentDateTime { get; private set; }
        [Required]
        public decimal Amount { get; private set; }

        [Required]
        public Guid RentalId { get; private set; } // 1:1
        [Required]
        public Rental Rental { get; private set; }

        public Payment(Guid id,
            string stripeTransactionId,
            Rental rental,
            Guid rentalId,
            DateTime paymentDateTime, 
            decimal amount)
            : base(id)
        {
            StripeTransactionId = stripeTransactionId;
            RentalId = rentalId;
            Rental = rental;
            Amount = amount;
            PaymentDateTime = paymentDateTime;
        }
    }
}
