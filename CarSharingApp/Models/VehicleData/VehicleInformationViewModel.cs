using CarSharingApp.Models.RatingData;
using CarSharingApp.Models.VehicleData.Includes;

namespace CarSharingApp.Models.VehicleData
{
    public class VehicleInformationViewModel
    {
        public int Id { get; set; }

        public string OwnerUsername { get; set; }
        public int OwnerId { get; set; }

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
        public Location Location { get; set; }

        public DateTime PublishedTime { get; set; }

        public VehicleRatingViewModel Rating { get; set; }

        public int TimesOrdered { get; set; }
    }
}
