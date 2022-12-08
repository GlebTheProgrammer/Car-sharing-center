using CarSharingApp.Models.OrderData;
using CarSharingApp.Payment;
using CarSharingApp.Repository.Interfaces;

namespace CarSharingApp.Order
{
    public sealed class CompleatedOrderProvider : IOrderProvider
    {
        private readonly IRepositoryManager _repositoryManager;

        public CompleatedOrderProvider(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public OrderModel Provide(PaymentUrlModel payment, int orderedUserId)
        {
            DateTime orderMadeTime = CalculateOrderMadeTime(payment.StartDay, payment.StartHour);
            int paidTimeInMinutes = CalculatePaidTimeInMinutes(orderMadeTime, int.Parse(payment.EndDay), int.Parse(payment.EndHour));

            OrderModel newOrder = new OrderModel()
            {
                IsActive = true,

                OrderedUserId = orderedUserId,
                OrderedVehicleId = payment.VehicleId,
                VehicleOwnerId = _repositoryManager.VehiclesRepository.GetVehicleById(payment.VehicleId).OwnerId,

                Price = decimal.Parse(payment.Amount),

                OrderMadeTime = orderMadeTime,
                PaidTimeInMinutes = paidTimeInMinutes,
                ExpiredTime = CalculateOrderExpirationDate(orderMadeTime, paidTimeInMinutes)
            };

            return newOrder;
        }

        private DateTime CalculateOrderMadeTime(string startDay, string startHour)
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

        private int CalculatePaidTimeInMinutes(DateTime orderMadeTime, int endDay, int endHour)
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

        private DateTime CalculateOrderExpirationDate(DateTime orderStartTime, int paidTimeInMinutes)
        {
            return orderStartTime.AddMinutes(paidTimeInMinutes);
        }
    }
}
