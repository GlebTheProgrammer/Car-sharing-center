using AutoMapper;
using CarSharingApp.Models.MongoView;
using CarSharingApp.Payment;
using CarSharingApp.Repository.MongoDbRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    [Authorize]
    public class VehicleInformationController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        private readonly IPaymentSessionProvider _paymentSessionProvider;
        private readonly IMapper _mapper;

        public VehicleInformationController(MongoDbService mongoDbService, IMapper mapper, IPaymentSessionProvider paymentSessionProvider)
        {
            _mapper = mapper;
            _paymentSessionProvider = paymentSessionProvider;

            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index(string vehicleId)
        {
            VehicleInformationModel vehicleInformationModel = await _mongoDbService.GetVehicleInformation(vehicleId);

            return View(vehicleInformationModel);
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
