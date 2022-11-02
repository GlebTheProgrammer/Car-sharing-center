namespace CarSharingApp.Models.OrderData
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int OrderedUserId { get; set; }
        public int OrderedVehicleId { get; set; }

        public decimal Price { get; set; } // Цена, вычисленная системой
        public decimal AditionalPrice { get; set; } // Цена, которую пользователь заплатил за превышение лимита

        public DateTime OrderMadeTime { get; set; } // Во сколько был оформлен заказ
        public DateTime ExpiredTime { get; set; } // Время окончания заказа (до какого момента времени автомобиль был оплачен)
        public DateTime? OrderFinishedTime { get; set; } // Во сколько пользователь закончил заказ из своего личного кабинета
    }
}
