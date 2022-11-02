namespace CarSharingApp.Models.RatingData
{
    // Предоставляется пользователю после остановки пользования транспортным средством в качестве просьбы оценить автомобиль
    public class ProvideRatingViewModel
    {
        public double Condition { get; set; }
        public double FuelConsumption { get; set; }
        public double EasyToDrive { get; set; }
        public double FamilyFriendly { get; set; }
        public double SUV { get; set; }
    }
}
