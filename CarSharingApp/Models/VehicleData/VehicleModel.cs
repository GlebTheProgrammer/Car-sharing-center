using CarSharingApp.Models.VehicleData.Includes;

namespace CarSharingApp.Models.VehicleData
{
    public class VehicleModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public int RatingId { get; set; }

        public DateTime PublishedTime { get; set; }

        public int TimesOrdered { get; set; }

        // Заполняется на странице
        public string Name { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public Tariff Tariff { get; set; }
        public string Image { get; set; }
        public int ProductionYear { get; set; }
        public int MaxSpeed { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public Drivetrain Drivetrain { get; set; }
        public FuelType FuelType { get; set; }
        public Transmission Transmission { get; set; }
        public Engine Engine { get; set; }
        public string VIN { get; set; }
        public Location Location { get; set; }

        // Активна ли техника или нет (меняется пользователем в личном кабинете)
        public bool IsPublished { get; set; }
        public bool IsOrdered { get; set; }
        
    }
}
