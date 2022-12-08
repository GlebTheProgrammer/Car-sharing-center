using CarSharingApp.Models.OrderData;
using CarSharingApp.Payment;

namespace CarSharingApp.Order
{
    public interface IOrderProvider
    {
        public OrderModel Provide(PaymentUrlModel payment, int orderedUserId);
    }
}
