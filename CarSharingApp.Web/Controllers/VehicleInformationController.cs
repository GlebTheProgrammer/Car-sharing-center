using CarSharingApp.Application.Contracts.Payment;
using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public class VehicleInformationController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;
        private readonly IStripePlatformPublicApiClient _stripePlatformClient;
        private readonly IRentalServicePublicApiClient _rentalServiceClient;

        public VehicleInformationController(IVehicleServicePublicApiClient vehicleServiceClient, 
                                            IStripePlatformPublicApiClient stripePlatformClient,
                                            IRentalServicePublicApiClient rentalServiceClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
            _stripePlatformClient = stripePlatformClient;
            _rentalServiceClient = rentalServiceClient;
        }

        public async Task<IActionResult> Index(string vehicleId)
        {
            var response = await _vehicleServiceClient.GetVehicleInformation(Guid.Parse(vehicleId));

            response.EnsureSuccessStatusCode();

            VehicleInformationResponse responseModel = await response.Content.ReadFromJsonAsync<VehicleInformationResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return View(responseModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(StripePaymentSessionRequest paymentRequest)
        {
            string hostedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";

            var stripeSessionUrlResponse = await _stripePlatformClient.GetStripeSessionUrl(
                GenerateNewStripePaymentSessionUrlRequest(
                    request: paymentRequest,
                    successUrl: hostedUrl + Url.Action("SuccessfulPayment", paymentRequest),
                    cancelationUrl: hostedUrl + Url.Action("CancelledPayment", paymentRequest)));

            string responseContent = await stripeSessionUrlResponse.Content.ReadAsStringAsync();

            StripePaymentSessionResponse stripeSessionUrl = JsonSerializer.Deserialize<StripePaymentSessionResponse>(responseContent) 
                ?? throw new NotImplementedException(nameof(CreateCheckoutSession));

            Response.Headers.Add("Location", stripeSessionUrl.SessionUrl);

            return Redirect(stripeSessionUrl.SessionUrl);
        }

        public async Task<IActionResult> SuccessfulPayment(StripePaymentSessionRequest completedPayment, string sessionId)
        {
            var paymentDetailsResponse = await _stripePlatformClient.GetStripePaymentDetails(sessionId);
            string paymentDetailsResponseContent = await paymentDetailsResponse.Content.ReadAsStringAsync();

            StripePaymentDetailsResponse stripePaymentDetails = JsonSerializer.Deserialize<StripePaymentDetailsResponse>(paymentDetailsResponseContent)
                ?? throw new NotImplementedException(nameof(SuccessfulPayment));

            var submitNewRentalResponse = await _rentalServiceClient.CreateRentalRequest(
                GenerateNewRequest(
                    request: completedPayment,
                    paymentResponse: stripePaymentDetails));

            string submitNewRentalResponseContent = await submitNewRentalResponse.Content.ReadAsStringAsync();

            RentalResponse newRental = JsonSerializer.Deserialize<RentalResponse>(submitNewRentalResponseContent)
                ?? throw new NotImplementedException(nameof(SuccessfulPayment));

            HttpContext.Session.SetString("CompletedPayment", "true");

            return RedirectToAction("Index", "Catalog");
        }

        public IActionResult CancelledPayment(StripePaymentSessionRequest cancelledPayment)
        {
            HttpContext.Session.SetString("CancelledPayment", "true");

            return RedirectToAction("Index","VehicleInformation", new { vehicleId = cancelledPayment.VehicleId });
        }

        #region Partial section starts here

        public IActionResult RentOrderPartial(string vehicleId, string vehicleName, string vehicleOwnerId, string tariffPerHour, string tariffPerDay)
        {
            var viewModel = new StripePaymentSessionRequest()
            {
                VehicleId = vehicleId,
                VehicleName = vehicleName,
                VehicleOwnerId = vehicleOwnerId,
                TariffPerHour = decimal.Parse(tariffPerHour),
                TariffPerDay = decimal.Parse(tariffPerDay)
            };

            return PartialView("_RentOrder", viewModel);
        }

        #endregion

        #region Models parsing

        private CreateNewRentalRequest GenerateNewRequest(StripePaymentSessionRequest request, StripePaymentDetailsResponse paymentResponse)
        {
            DateTime rentalStartsDateTime;
            DateTime rentalEndsDateTime;

            int rentalStartsMonth = DateTime.ParseExact(request.StartMonth, "MMM", CultureInfo.InvariantCulture).Month;
            int rentalEndsMonth = DateTime.ParseExact(request.EndMonth, "MMM", CultureInfo.InvariantCulture).Month;

            rentalStartsDateTime = new DateTime(year: DateTime.Now.Year, month: rentalStartsMonth, day: int.Parse(request.StartDay), hour: int.Parse(request.StartHour), minute: 0, second: 0);

            if (rentalStartsDateTime < DateTime.Now) // Rental has to start in the next year
            {
                rentalStartsDateTime = new DateTime(year: DateTime.Now.Year + 1, month: rentalStartsMonth, day: int.Parse(request.StartDay), hour: int.Parse(request.StartHour), minute: 0, second: 0);
            }

            if (rentalEndsMonth < rentalStartsMonth) // Rental ends after new year
            {
                rentalEndsDateTime = new DateTime(year: DateTime.Now.Year + 1, month: rentalEndsMonth, day: int.Parse(request.EndDay), hour: int.Parse(request.EndHour), minute: 0, second: 0);
            }
            else // Rental ends in the same year when started
            {
                rentalEndsDateTime = new DateTime(year: DateTime.Now.Year, month: rentalEndsMonth, day: int.Parse(request.EndDay), hour: int.Parse(request.EndHour), minute: 0, second: 0);
            }

            return new CreateNewRentalRequest(
                VehicleId: request.VehicleId,
                VehicleName: request.VehicleName,
                VehicleOwnerId: request.VehicleOwnerId,
                PaymentAmount: paymentResponse.Amount / 100,
                PaymentDateTime: paymentResponse.PaymentDateTime,
                RentalStartsDateTime: rentalStartsDateTime,
                RentalEndsDateTime: rentalEndsDateTime,
                StripePaymentId: paymentResponse.PaymentId);
        }

        private StripePaymentSessionUrlRequest GenerateNewStripePaymentSessionUrlRequest(
            StripePaymentSessionRequest request, string successUrl, string cancelationUrl)
        {
            return new StripePaymentSessionUrlRequest(
                VehicleId: request.VehicleId,
                VehicleOwnerId: request.VehicleOwnerId,
                VehicleName: request.VehicleName,
                Amount: request.Amount,
                TariffPerHour: request.TariffPerHour,
                TariffPerDay: request.TariffPerDay,
                StartHour: request.StartHour,
                StartDay: request.StartDay,
                StartMonth: request.StartMonth,
                EndHour: request.EndHour,
                EndDay: request.EndDay,
                EndMonth: request.EndMonth,
                SuccessUrl: successUrl,
                CancelationUrl: cancelationUrl);
        }

        #endregion
    }
}
