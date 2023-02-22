using ErrorOr;

namespace CarSharingApp.Domain.ValidationErrors
{
    public static partial class DomainErrors
    {
        public static class Payment
        {
            public static Error InvalidPaymentDateTime => Error.Validation(
            code: "Payment.InvalidPaymentDateTime.PaymentDateTime",
            description: "Payment DateTime can not be later then current DateTime.");

            public static Error InvalidPaymentAmount => Error.Validation(
            code: "Payment.InvalidPaymentAmount.Amount",
            description: $"Payment amount must be at least {Entities.Payment.MinPaymentAmout}$ and at most {Entities.Payment.MaxPaymentAmout}$.");

            public static Error InvalidStripeTransactionId => Error.Validation(
            code: "Payment.InvalidStripeTransactionId.StripeTransactionId",
            description: $"Stripe payment identifier is not valid.");
        }
    }
}
