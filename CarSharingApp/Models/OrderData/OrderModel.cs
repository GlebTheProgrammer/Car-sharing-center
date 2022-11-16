namespace CarSharingApp.Models.OrderData
{
    public class OrderModel
    {
        public bool IsActive { get; set; } // Состояние заказа

        public int Id { get; set; } // ID заказа
        public int OrderedUserId { get; set; } // ID заказавшего пользователя
        public int OrderedVehicleId { get; set; } // ID заказанной техники
        public int VehicleOwnerId { get; set; } // ID владельца техники

        public decimal Price { get; set; } // Цена, вычисленная системой

        public DateTime OrderMadeTime { get; set; } // Во сколько был оформлен заказ
        public DateTime ExpiredTime { get; set; } // Время окончания заказа (до какого момента времени автомобиль был оплачен)
        public int PaidTimeInMinutes { get; set; } // Оплаченное время
    }
}
