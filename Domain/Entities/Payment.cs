using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValidationErrors;
using ErrorOr;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Payment : Entity
    {
        public const int MinPaymentAmout = 1;
        public const int MaxPaymentAmout = 100000;

        [Required]
        [MaxLength(255)]
        public string StripeTransactionId { get; private set; }
        [Required]
        public DateTime PaymentDateTime { get; private set; }
        [Required]
        public decimal Amount { get; private set; }

        public Guid RentalId { get; private set; } // 1:1
        public Rental? Rental { get; private set; }

        private Payment(Guid id,
            string stripeTransactionId,
            DateTime paymentDateTime, 
            decimal amount)
            : base(id)
        {
            StripeTransactionId = stripeTransactionId;
            Amount = amount;
            PaymentDateTime = paymentDateTime;
        }

        public static ErrorOr<Payment> Create(
            string stripeTransactionId,
            DateTime paymentDateTime,
            decimal amount,
            Guid? id = null)
        {
            List<Error> errors = new();

            if (DateTime.UtcNow < paymentDateTime)
            {
                errors.Add(DomainErrors.Payment.InvalidPaymentDateTime);
            }
            if (amount is < MinPaymentAmout or > MaxPaymentAmout)
            {
                errors.Add(DomainErrors.Payment.InvalidPaymentAmount);
            }
            if (!stripeTransactionId.StartsWith("pi_") || stripeTransactionId.Length > 255)
            {
                errors.Add(DomainErrors.Payment.InvalidStripeTransactionId);
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Payment(
                id ?? Guid.NewGuid(),
                stripeTransactionId,
                paymentDateTime,
                amount);
        }

        public static Payment CombinePaymentWithARental(Payment payment, Rental rental)
        {
            payment.Rental = rental;

            return payment;
        }
    }
}
