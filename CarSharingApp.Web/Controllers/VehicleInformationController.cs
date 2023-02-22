using CarSharingApp.Application.Contracts.ErrorType;
using CarSharingApp.Application.Contracts.Payment;
using CarSharingApp.Application.Contracts.Rental;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public class VehicleInformationController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;
        private readonly IStripePlatformPublicApiClient _stripePlatformClient;

        public VehicleInformationController(IVehicleServicePublicApiClient vehicleServiceClient, 
                                            IStripePlatformPublicApiClient stripePlatformClient)
        {
            _vehicleServiceClient = vehicleServiceClient;
            _stripePlatformClient = stripePlatformClient;
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
            //await _mongoDbService

            //var compleatedOrder = _orderProvider.Provide(payment, (int)_userStatusProvider.GetUserId());

            //_repositoryManager.OrdersRepository.AddNewOrder(compleatedOrder);

            //_repositoryManager.VehiclesRepository.ChangeVehicleIsOrderedState(payment.VehicleId, true);

            //_userStatusProvider.ChangeCompletedPaymentProcessState(true);

            //_repositoryManager.ClientsRepository.IncreaseClientsVehiclesSharedAndOrderedCount((int)_userStatusProvider.GetUserId(), _repositoryManager.VehiclesRepository.GetVehicleById(payment.VehicleId).OwnerId);

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

        private CreateNewRentalRequest GenerateNewRequest(StripePaymentSessionRequest request)
        {
            throw new NotImplementedException();
            //return new CreateNewRentalRequest(
            //    VehicleId: request.VehicleId,
            //    VehicleName: request.VehicleName,
            //    VehicleOwnerId: request.VehicleOwnerId,
            //    Amount: decimal.Parse(request.Amount),
                
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
