using CarSharingApp.Application.Contracts.Payment;
using CarSharingApp.PublicApi.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace CarSharingApp.PublicApi.Controllers
{
    public class PaymentsController : ApiController
    {
        private const string StripePaymentProcessImage = "https://www.hotellinksolutions.com/images/learning-center/payment-101.jpg";

        public PaymentsController(IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration["StripeConfig:SecretKey"];
        }

        [HttpPost]
        [Authorize]
        public IActionResult GenerateStripeSession(StripePaymentSessionUrlRequest payment)
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
                SuccessUrl = payment.SuccessUrl + "&sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = payment.CancelationUrl,
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            return Ok(MapStripePaymentSessionResponse(session));
        }

        private StripePaymentSessionResponse MapStripePaymentSessionResponse(Stripe.Checkout.Session session)
        {
            return new StripePaymentSessionResponse(
                SessionUrl: session.Url);
        }
    }
}
