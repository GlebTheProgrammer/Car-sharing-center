using Stripe.Checkout;

namespace CarSharingApp.Payment
{
    public interface IPaymentSessionProvider
    {
        public Session Provide(PaymentModel payment, string successUrl, string cancelationUrl);
    }
}
