using AutoMapper;
using CarSharingApp.Models.RatingData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Order;
using CarSharingApp.Payment;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSharingApp.Controllers
{
    public class VehicleInformationController : Controller
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IPaymentSessionProvider _paymentSessionProvider;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _userStatusProvider;

        public VehicleInformationController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider userStatusProvider, 
                                        IPaymentSessionProvider paymentSessionProvider, IOrderProvider orderProvider)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _userStatusProvider = userStatusProvider;
            _paymentSessionProvider = paymentSessionProvider;
            _orderProvider = orderProvider;
        }

        public IActionResult Index(int vehicleId)
        {
            if(_userStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (_userStatusProvider.GetUserRole() != UserRole.Client)
            {
                _userStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            VehicleModel vehicle = _repositoryManager.VehiclesRepository.GetVehicleById(vehicleId);

            var vehicleViewModel = _mapper.Map<VehicleInformationViewModel>(vehicle);

            vehicleViewModel.OwnerUsername = _repositoryManager.ClientsRepository.GetClientUsername(vehicle.OwnerId);
            vehicleViewModel.Rating = _mapper.Map<VehicleRatingViewModel>(_repositoryManager.RatingRepository.GetVehicleRatingById(vehicle.RatingId));

            return View(vehicleViewModel);
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

        public IActionResult SuccessfulPayment(PaymentUrlModel payment)
        {
            var compleatedOrder = _orderProvider.Provide(payment, (int)_userStatusProvider.GetUserId());

            _repositoryManager.OrdersRepository.AddNewOrder(compleatedOrder);

            _repositoryManager.VehiclesRepository.ChangeVehicleIsOrderedState(payment.VehicleId, true);

            _userStatusProvider.ChangeCompletedPaymentProcessState(true);

            _repositoryManager.ClientsRepository.IncreaseClientsVehiclesSharedAndOrderedCount((int)_userStatusProvider.GetUserId(), _repositoryManager.VehiclesRepository.GetVehicleById(payment.VehicleId).OwnerId);

            return RedirectToAction("Index", "Catalog");
        }

        public IActionResult CancelledPayment(PaymentUrlModel payment)
        {
            _userStatusProvider.ChangeCanceledPaymentProcessState(true);

            return RedirectToAction("Index","VehicleInformation", new { vehicleId = payment.VehicleId });
        }

        // Partial section starts here

        public IActionResult RentOrderPartial(int vehicleId, string vehicleName, string tariffPerHour, string tariffPerDay)
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
