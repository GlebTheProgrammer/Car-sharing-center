using CarSharingApp.Models.Mongo.Enums;
using CarSharingApp.Models.Mongo.Includes;

namespace CarSharingApp.Models.MongoView
{
    public class VehicleInformationModel
    {
        public string VehicleId { get; set; } = null!;

        public string OwnerUsername { get; set; } = null!;
        public int OwnerId { get; set; }

        public string Name { get; set; } = null!; 
        public string BriefDescription { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Tariff Tariff { get; set; } = new Tariff();
        public string Image { get; set; } = null!;
        public int ProductionYear { get; set; }
        public int MaxSpeed { get; set; }
        public string ExteriorColor { get; set; } = null!;
        public string InteriorColor { get; set; } = null!;
        public Drivetrain Drivetrain { get; set; }
        public FuelType FuelType { get; set; }
        public Transmission Transmission { get; set; }
        public Engine Engine { get; set; }
        public Location Location { get; set; } = new Location();
        public DateTime PublishedTime { get; set; }

        public int TimesOrdered { get; set; }

        public RatingInformationModel Rating { get; set; } = new RatingInformationModel();

    }
}
