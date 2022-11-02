namespace CarSharingApp.Models.RatingData
{
    // Предоставляется пользователю на странице с информацией о транспортном средстве
    public class VehicleRatingViewModel
    {
        public double Condition { get; set; }
        public double FuelConsumption { get; set; }
        public double EasyToDrive { get; set; }
        public double FamilyFriendly { get; set; }
        public double SUV { get; set; }

        public double Overall { get; set; }

        public int ReviewsCount { get; set; }
    }
}
