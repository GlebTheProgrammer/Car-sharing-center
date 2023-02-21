using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Services
{
    public class PaymentsService : IPaymentsService
    {
        public ErrorOr<Payment> From(CreateNewRentalRequest request)
        {
            return Payment.Create(
                stripeTransactionId: request.StripeTransactionId,
                paymentDateTime: request.PaymentDateTime,
                request.Amount);
        }
    }
}
