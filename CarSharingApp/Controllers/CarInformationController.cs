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

                OrderedUserId = (int)userStatusProvider.GetUserId(),
                OrderedVehicleId = vehicleId,
                VehicleOwnerId = vehiclesRepository.GetVehicleById(vehicleId).OwnerId,

                Price = decimal.Parse(orderPrice),

                OrderMadeTime = orderMadeTime,
                PaidTimeInMinutes = paidTimeInMinutes,
                ExpiredTime = CalculateOrderExpiredTime(orderMadeTime, paidTimeInMinutes)
            };

            // Добавляем новый заказ в список
            ordersRepository.AddNewOrder(newOrder);

            // Меняем состояние автомобиля в списке и файле ( Меняем состояние IsOrdered на True и количество заказов на автомобиль в случае передачи true)
            vehiclesRepository.ChangeVehicleIsOrderedState(vehicleId, true);

            // Статус в True для отправки пользователю соотвтствующее сообщение 
            userStatusProvider.ChangeCompletedPaymentProcessState(true);

            clientsRepository.IncreaseClientsVehiclesSharedAndOrderedCount((int)userStatusProvider.GetUserId(), vehiclesRepository.GetVehicleById(vehicleId).OwnerId);

            return RedirectToAction("Index", "CarSharing");
        }

        public IActionResult CancelledPayment(int vehicleId)
        {
            userStatusProvider.ChangeCanceledPaymentProcessState(true);

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
