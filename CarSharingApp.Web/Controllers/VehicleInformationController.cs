using CarSharingApp.Application.Contracts.Payment;
using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using CarSharingApp.Web.Extensions;
using CarSharingApp.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Web;

namespace CarSharingApp.Controllers
{
    [Authorize]
    [Route("vehicle")]
    public sealed class VehicleInformationController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;
        private readonly IStripePlatformPublicApiClient _stripePlatformClient;
        private readonly IRentalServicePublicApiClient _rentalServiceClient;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<VehicleInformationController> _logger;

        public VehicleInformationController(IVehicleServicePublicApiClient vehicleServiceClient, 
                                            IStripePlatformPublicApiClient stripePlatformClient,
                                            IRentalServicePublicApiClient rentalServiceClient,
                                            IWebHostEnvironment webHostEnvironment,
                                            ILogger<VehicleInformationController> logger)
        {
            _vehicleServiceClient = vehicleServiceClient;
            _stripePlatformClient = stripePlatformClient;
            _rentalServiceClient = rentalServiceClient;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        [HttpGet]
        [Route("information")]
        public async Task<IActionResult> Index([FromQuery] string vehicleId)
        {
            var response = await _vehicleServiceClient.GetVehicleInformation(Guid.Parse(vehicleId));

            response.EnsureSuccessStatusCode();

            VehicleInformationResponse responseModel = await response.Content.ReadFromJsonAsync<VehicleInformationResponse>()
                ?? throw new NullReferenceException(nameof(responseModel));

            return View(responseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        [Route("payment/session")]
        public async Task<IActionResult> CreateCheckoutSession([FromForm] StripePaymentSessionRequest paymentRequest)
        {
            _logger.LogInformation("CreateCheckoutSession rentalStartsLocalDateTime = {DateTime}", paymentRequest.RentalStartsDateTimeLocalStr);

            DateTime rentalStartsLocalDateTime = MyCustomDateTimeProvider.ParseFromViewIntoCurrentCustomerLocalDateTime(paymentRequest.RentalStartsDateTimeLocalStr);
            DateTime rentalEndsLocalDateTime = MyCustomDateTimeProvider.ParseFromViewIntoCurrentCustomerLocalDateTime(paymentRequest.RentalEndsDateTimeLocalStr);

            _logger.LogInformation("CreateCheckoutSession rentalStartsLocalDateTime = {DateTime}", rentalStartsLocalDateTime);

            string hostedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";

            string successResultUrl = HttpUtility.UrlEncode(hostedUrl + Url.Action("SuccessfulPayment", paymentRequest) 
                ?? throw new ArgumentNullException(nameof(paymentRequest)));
            string cancelledResultUrl = HttpUtility.UrlEncode(hostedUrl + Url.Action("CancelledPayment", paymentRequest) 
                ?? throw new ArgumentNullException(nameof(paymentRequest)));

            var stripeSessionUrlResponse = await _stripePlatformClient.GetStripeSessionUrl(
                GenerateNewStripePaymentSessionUrlRequest(
                    request: paymentRequest,
                    rentalStartsUtcDateTime: rentalStartsLocalDateTime,
                    rentalEndsUtcDateTime: rentalEndsLocalDateTime,
                    successUrl: successResultUrl,
                    cancelationUrl: cancelledResultUrl));

            string responseContent = await stripeSessionUrlResponse.Content.ReadAsStringAsync();

            StripePaymentSessionResponse stripeSessionUrl = JsonSerializer.Deserialize<StripePaymentSessionResponse>(responseContent) 
                ?? throw new NotImplementedException(nameof(CreateCheckoutSession));

            Response.Headers.Add("Location", stripeSessionUrl.SessionUrl);

            return Redirect(stripeSessionUrl.SessionUrl);
        }

        [HttpGet]
        [Route("payment/compleated")]
        public async Task<IActionResult> SuccessfulPayment([FromQuery] StripePaymentSessionRequest completedPayment, 
                                                           [FromQuery] string sessionId)
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

        [HttpGet]
        [Route("payment/cancelled")]
        public IActionResult CancelledPayment([FromQuery] StripePaymentSessionRequest cancelledPayment)
        {
            HttpContext.Session.SetString("CancelledPayment", "true");

            return RedirectToAction("Index","VehicleInformation", new { vehicleId = cancelledPayment.VehicleId });
        }

        #region Partial section starts here

        [Route("rentalPartial")]
        public IActionResult RentOrderPartial([FromForm] string vehicleId, 
                                              [FromForm] string vehicleName, 
                                              [FromForm] string vehicleOwnerId, 
                                              [FromForm] string tariffPerHour, 
                                              [FromForm] string tariffPerDay)
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

        #region Models parsing section starts here

        [NonAction]
        private CreateNewRentalRequest GenerateNewRequest(StripePaymentSessionRequest request, StripePaymentDetailsResponse paymentResponse)
        {
            DateTime rentalStartsUtcDateTime = MyCustomDateTimeProvider.ParseFromLocalToUtcDateTime(request.RentalStartsDateTimeLocalStr);
            DateTime rentalEndsUtcDateTime = MyCustomDateTimeProvider.ParseFromLocalToUtcDateTime(request.RentalEndsDateTimeLocalStr);

            _logger.LogInformation("GenerateNewRequest rentalStartsUtcDateTime = {DateTime}", rentalStartsUtcDateTime);

            return new CreateNewRentalRequest(
                VehicleId: request.VehicleId,
                VehicleName: request.VehicleName,
                VehicleOwnerId: request.VehicleOwnerId,
                PaymentAmount: paymentResponse.Amount / 100,
                PaymentDateTime: paymentResponse.PaymentDateTime,
                RentalStartsDateTime: rentalStartsUtcDateTime/*.ToString(CultureInfo.InvariantCulture)*/,
                RentalEndsDateTime: rentalEndsUtcDateTime/*.ToString(CultureInfo.InvariantCulture)*/,
                StripePaymentId: paymentResponse.PaymentId);
        }

        [NonAction]
        private StripePaymentSessionUrlRequest GenerateNewStripePaymentSessionUrlRequest(
            StripePaymentSessionRequest request, string successUrl, string cancelationUrl, DateTime rentalStartsUtcDateTime, DateTime rentalEndsUtcDateTime)
        {
            return new StripePaymentSessionUrlRequest(
                VehicleId: request.VehicleId,
                VehicleOwnerId: request.VehicleOwnerId,
                VehicleName: request.VehicleName,
                Amount: request.Amount,
                TariffPerHour: request.TariffPerHour,
                TariffPerDay: request.TariffPerDay,
                RentalStartsDateTimeUTC: rentalStartsUtcDateTime.ToString(CultureInfo.InvariantCulture),
                RentalEndsDateTimeUTC: rentalEndsUtcDateTime.ToString(CultureInfo.InvariantCulture),
                SuccessUrl: successUrl,
                CancelationUrl: cancelationUrl);
        }

        #endregion
    }
}
