using AutoMapper;
using CarSharingApp.Models.OrderData;
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
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserStatusProvider _userStatusProvider;

        public CarInformationController(IRepositoryManager repositoryManager, IMapper mapper, ICurrentUserStatusProvider userStatusProvider)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _userStatusProvider = userStatusProvider;
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
        public ActionResult CreateCheckoutSession(string amount, string vehicleName, string startMonth, string endMonth, 
                                                  string startDay, string endDay, string startHour, string endHour, int vehicleId)
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
                SuccessUrl = $"https://localhost:44362/CarInformation/SuccessfulPayment?vehicleId={vehicleId}&orderPrice={amount}&startDay={startDay}&startHour={startHour}&endDay={endDay}&endHour={endHour}",
                CancelUrl = $"https://localhost:44362/CarInformation/CancelledPayment?vehicleId={vehicleId}",

            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return Redirect(session.Url);
        }

        public IActionResult SuccessfulPayment(int vehicleId, string orderPrice, string startDay, string startHour, string endDay, string endHour)
        {
            DateTime orderMadeTime = CalculateOrderMadeTime(startDay, startHour);
            int paidTimeInMinutes = CalculatePaidTimeInMinutes(orderMadeTime, int.Parse(endDay), int.Parse(endHour));

            OrderModel newOrder = new OrderModel()
            {
                IsActive = true,

                OrderedUserId = (int)_userStatusProvider.GetUserId(),
                OrderedVehicleId = vehicleId,
                VehicleOwnerId = _repositoryManager.VehiclesRepository.GetVehicleById(vehicleId).OwnerId,

                Price = decimal.Parse(orderPrice),

                OrderMadeTime = orderMadeTime,
                PaidTimeInMinutes = paidTimeInMinutes,
                ExpiredTime = CalculateOrderExpiredTime(orderMadeTime, paidTimeInMinutes)
            };


            _repositoryManager.OrdersRepository.AddNewOrder(newOrder);

            _repositoryManager.VehiclesRepository.ChangeVehicleIsOrderedState(vehicleId, true);

            _userStatusProvider.ChangeCompletedPaymentProcessState(true);

            _repositoryManager.ClientsRepository.IncreaseClientsVehiclesSharedAndOrderedCount((int)_userStatusProvider.GetUserId(), _repositoryManager.VehiclesRepository.GetVehicleById(vehicleId).OwnerId);

            return RedirectToAction("Index", "CarSharing");
        }

        public IActionResult CancelledPayment(int vehicleId)
        {
            _userStatusProvider.ChangeCanceledPaymentProcessState(true);

            return RedirectToAction("Index", vehicleId);
        }

        public DateTime CalculateOrderMadeTime(string startDay, string startHour)
        {
            // Заказ пользователь сделал: 23:59 31 декабря 2022 -> Нам придёт следующая дата: 00:00 1 января 2023  
            // Если делать влоб, и назначить Year как Datetime.now.Year - получим ошибку, тк дата будет
            // взята как 00:00 1 января 2022. Чтобы избежать ошибки - проверяем на час вперед, какой год
            // и если он равен тому, который показывает Datetime.now - берём его. Если же не равен - берём 
            // datetime.now + 1 год

            DateTime now = DateTime.Now;

            int year;
            int month;

            if (now.Year == now.AddHours(1).Year)
                year = now.Year;
            else
                year = now.AddHours(1).Year;

            if (now.Month == now.AddHours(1).Month)
                month = now.Month;
            else
                month = now.AddHours(1).Month;

            return new DateTime(year, month, int.Parse(startDay), int.Parse(startHour), 0, 0);
        }

        public int CalculatePaidTimeInMinutes(DateTime orderMadeTime, int endDay, int endHour)
        {
            int resultMinutes;

            if (endDay > orderMadeTime.Day && orderMadeTime.Hour > endHour) // 28 23:00 -> 30 1:00
            {
                resultMinutes = (endDay - orderMadeTime.Day - 1) * 24 * 60 + (24 - orderMadeTime.Hour + endHour) * 60;
                return resultMinutes;
            }

            if (endDay < orderMadeTime.Day && orderMadeTime.Hour > endHour) // 28 23:00 -> 1 1:00
            {
                resultMinutes = (DateTime.DaysInMonth(orderMadeTime.Year, orderMadeTime.Month) - orderMadeTime.Day - 1 + endDay) * 24 * 60 + (24 - orderMadeTime.Hour + endHour) * 60;
                return resultMinutes;
            }

            if (endDay < orderMadeTime.Day && orderMadeTime.Hour <= endHour) // 28 1:00 -> 1 23:00
            {
                resultMinutes = (DateTime.DaysInMonth(orderMadeTime.Year, orderMadeTime.Month) - orderMadeTime.Day + endDay) * 24 * 60 + (endHour - orderMadeTime.Hour) * 60;
                return resultMinutes;
            }
            else // 28 1:00 -> 30 23:00
            {
                resultMinutes = (endDay - orderMadeTime.Day) * 24 * 60 + (endHour - orderMadeTime.Hour) * 60;
                return resultMinutes;
            }
        }

        public DateTime CalculateOrderExpiredTime(DateTime orderStartTime, int paidTimeInMinutes)
        {
            return orderStartTime.AddMinutes(paidTimeInMinutes);
        }
    }
}
