using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Interfaces;
using CarSharingApp.Application.ServiceErrors;
using CarSharingApp.Domain.Abstractions;
using CarSharingApp.Domain.Entities;
using ErrorOr;

namespace CarSharingApp.Application.Services
{
    public sealed class PaymentsService : IPaymentsService
    {
        private readonly IRepository<Payment> _paymentRepository;

        public PaymentsService(IRepository<Payment> paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<ErrorOr<Payment>> GetByRentalIdAsync(Guid rentalId)
        {
            var result = await _paymentRepository.GetAsync(p => p.RentalId == rentalId);

            if (result != null)
                return result;
            else
                return ApplicationErrors.Payment.NotFound;
        }

        public ErrorOr<Payment> From(CreateNewRentalRequest request)
        {
            return Payment.Create(
                stripeTransactionId: request.StripePaymentId,
                paymentDateTime: request.PaymentDateTime,
                request.PaymentAmount);
        }
    }
}
