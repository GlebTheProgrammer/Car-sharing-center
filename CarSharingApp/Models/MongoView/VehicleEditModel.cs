using CarSharingApp.Models.Mongo.Enums;
using CarSharingApp.Models.Mongo.Includes;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.MongoView
{
    public class VehicleEditModel
    {
        // Protected fields

        public string VehicleId { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int ProductionYear { get; set; }
        public int MaxSpeed { get; set; }
        public string ExteriorColor { get; set; } = null!; 
        public string InteriorColor { get; set; } = null!;
        public Drivetrain Drivetrain { get; set; }
        public FuelType FuelType { get; set; }
        public Transmission Transmission { get; set; }
        public Engine Engine { get; set; }
        public string VIN { get; set; } = null!;

        // Fields can be changed

        public Location Location { get; set; } = new Location();

        [Required(ErrorMessage = "Brief description field is required")]
        [MinLength(10, ErrorMessage = "Brief description can't be less then 10 symbols")]
        [MaxLength(40, ErrorMessage = "Brief description can't be longer then 40 symbols")]
        public string BriefDescription { get; set; } = null!;

        [Required(ErrorMessage = "Description field is required")]
        [MinLength(30, ErrorMessage = "Description can't be less then 30 symbols")]
        [MaxLength(255, ErrorMessage = "Brief description can't be longer then 255 symbols")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Tariff field is required")]
        public Tariff Tariff { get; set; } = new Tariff();
    }
}
