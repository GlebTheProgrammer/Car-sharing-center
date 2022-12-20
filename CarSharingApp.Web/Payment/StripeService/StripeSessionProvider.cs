using Stripe.Checkout;

namespace CarSharingApp.Payment.StripeService
{
    public sealed class StripeSessionProvider : IPaymentSessionProvider
    {
        private const string StripePaymentProcessImage = "https://www.hotellinksolutions.com/images/learning-center/payment-101.jpg";


        public Session Provide(PaymentModel payment, string successUrl, string cancelationUrl)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = int.Parse(payment.Amount) * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Order for {payment.VehicleName}",
                                Images = new List<string> { StripePaymentProcessImage },
                                Description = $"You are going to pay for the use of the «{payment.VehicleName}» transport within the specified period: " +
                                $"from {payment.StartMonth} {payment.StartDay}, {payment.StartHour}:00 till {payment.EndMonth} {payment.EndDay}, {payment.EndHour}:00. " +
                                $"For the overdue time, the customer is responsible to the vehicle's owner."
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelationUrl,
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            return session;
        }
    }
}
