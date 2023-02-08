using AutoMapper;
using CarSharingApp.Application.Contracts.Vehicle;
using CarSharingApp.Payment;
using CarSharingApp.Web.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class VehicleInformationController : Controller
    {
        private readonly IVehicleServicePublicApiClient _vehicleServiceClient;

        private readonly IPaymentSessionProvider _paymentSessionProvider;
        private readonly IMapper _mapper;

        public VehicleInformationController(IMapper mapper, IPaymentSessionProvider paymentSessionProvider, IVehicleServicePublicApiClient vehicleServiceClient)
        {
            _mapper = mapper;
            _paymentSessionProvider = paymentSessionProvider;

            _vehicleServiceClient = vehicleServiceClient;
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
        public ActionResult CreateCheckoutSession(PaymentModel paymentModel)
        {
            string hostedUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";
            var paymentUrlModel = _mapper.Map<PaymentUrlModel>(paymentModel);

            Stripe.Checkout.Session session = _paymentSessionProvider.Provide(
                payment: paymentModel,
                successUrl: hostedUrl + Url.Action("SuccessfulPayment", paymentUrlModel),
                cancelationUrl: hostedUrl + Url.Action("CancelledPayment", paymentUrlModel));

            Response.Headers.Add("Location", session.Url);

            return Redirect(session.Url);
        }

        public async Task<IActionResult> SuccessfulPayment(PaymentUrlModel payment)
        {
            //await _mongoDbService

            //var compleatedOrder = _orderProvider.Provide(payment, (int)_userStatusProvider.GetUserId());

            //_repositoryManager.OrdersRepository.AddNewOrder(compleatedOrder);

            //_repositoryManager.VehiclesRepository.ChangeVehicleIsOrderedState(payment.VehicleId, true);

            //_userStatusProvider.ChangeCompletedPaymentProcessState(true);

            //_repositoryManager.ClientsRepository.IncreaseClientsVehiclesSharedAndOrderedCount((int)_userStatusProvider.GetUserId(), _repositoryManager.VehiclesRepository.GetVehicleById(payment.VehicleId).OwnerId);

            return RedirectToAction("Index", "Catalog");
        }

        public IActionResult CancelledPayment(PaymentUrlModel payment)
        {
            HttpContext.Session.SetString("CancelledPayment", "true");

            return RedirectToAction("Index","VehicleInformation", new { vehicleId = payment.VehicleId });
        }

        // Partial section starts here

        public IActionResult RentOrderPartial(string vehicleId, string vehicleName, string tariffPerHour, string tariffPerDay)
        {
            var viewModel = new PaymentModel()
            {
                VehicleId = vehicleId,
                VehicleName = vehicleName,
                Tariff = new()
                {
                    TariffPerHour = decimal.Parse(tariffPerHour),
                    TariffPerDay = decimal.Parse(tariffPerDay)
                }
            };

            return PartialView("_RentOrder", viewModel);
        }
    }
}
