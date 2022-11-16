namespace CarSharingApp.Models.OrderData
{
    public class OrderInUserAccountViewModel
    {
        // Получаю из главной модели
        public int Id { get; set; }
        public DateTime OrderMadeTime { get; set; } // Во сколько был оформлен заказ
        public DateTime ExpiredTime { get; set; } // Время окончания заказа (до какого момента времени автомобиль был оплачен)
        public int PaidTimeInMinutes { get; set; } // Оплаченное время


        public int OrderedVehicleId { get; set; } // ID заказанной техники
        public string VehicleName { get; set; }
    }
}
