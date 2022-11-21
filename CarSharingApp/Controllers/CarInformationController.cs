using AutoMapper;
using CarSharingApp.Models.RatingData;
using CarSharingApp.Models.VehicleData;
using CarSharingApp.Repository.Interfaces;
using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Net;

namespace CarSharingApp.Controllers
{
    public class CarInformationController : Controller
    {
        private readonly IVehiclesRepository vehiclesRepository;
        private readonly IMapper mapper;
        private readonly IClientsRepository clientsRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly ICurrentUserStatusProvider userStatusProvider;

        public CarInformationController(IVehiclesRepository vehiclesRepository, IMapper mapper, IClientsRepository clientsRepository, IRatingRepository ratingRepository, 
                                        IOrdersRepository ordersRepository, ICurrentUserStatusProvider userStatusProvider)
        {
            this.vehiclesRepository = vehiclesRepository;
            this.mapper = mapper;
            this.clientsRepository = clientsRepository;
            this.ratingRepository = ratingRepository;
            this.ordersRepository = ordersRepository;
            this.userStatusProvider = userStatusProvider;
        }

        public IActionResult Index(int vehicleId)
        {
            if(userStatusProvider.HasUserLoggedOut())
                return RedirectToAction("Index", "Home");

            if (userStatusProvider.GetUserRole() != UserRole.Client)
            {
                userStatusProvider.ChangeUnauthorizedAccessState(true);
                return RedirectToAction("Index", "Home");
            }

            VehicleModel vehicle = vehiclesRepository.GetVehicleById(vehicleId);

            var vehicleViewModel = mapper.Map<VehicleInformationViewModel>(vehicle);

            vehicleViewModel.OwnerUsername = clientsRepository.GetClientUsername(vehicle.OwnerId);
            vehicleViewModel.Rating = mapper.Map<VehicleRatingViewModel>(ratingRepository.GetVehicleRatingById(vehicle.RatingId));
            vehicleViewModel.TimesOrdered = ordersRepository.GetNumberOfVehicleOrders(vehicleId);

            return View(vehicleViewModel);
        }

        [HttpPost]
        public ActionResult CreateCheckoutSession(string amount, string vehicleName, string startMonth, string endMonth, string startDay, string endDay, string startHour, string endHour)
        {

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = int.Parse(amount) * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Order for {vehicleName}",
                                Images = new List<string> { "https://www.hotellinksolutions.com/images/learning-center/payment-101.jpg" },
                                Description = $"You are going to pay for the use of the «{vehicleName}» transport within the specified period: from {startMonth} {startDay}, {startHour}:00 till {endMonth} {endDay}, {endHour}:00. For the overdue time, the customer is responsible to the vehicle's owner."
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:44362/CarInformation/SuccessfulPayment",
                CancelUrl = $"https://localhost:44362/CarInformation/CancelledPayment",

            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return Redirect(session.Url);
        }

        public IActionResult SuccessfulPayment()
        {
            return View();
        }

        public IActionResult CancelledPayment()
        {
            return View();
        }
    }
}
