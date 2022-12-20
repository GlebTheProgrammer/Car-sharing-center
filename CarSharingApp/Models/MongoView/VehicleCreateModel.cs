using CarSharingApp.Models.Mongo.Enums;
using CarSharingApp.Models.Mongo.Includes;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.MongoView
{
    public class VehicleCreateModel
    {
        [Required(ErrorMessage = "Vehicle name field is required")]
        [MinLength(1, ErrorMessage = "Vehicle name can't be empty")]
        [MaxLength(255, ErrorMessage = "Vehicle name cant't be longer then 255 symbols")]
        public string Name { get; set; } = null!;

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

        [Required(ErrorMessage = "Vehicle Image is required")]
        public string Image { get; set; } = null!;

        [Required(ErrorMessage = "Production year field is required")]
        [Range(1500, 2022, ErrorMessage = "Wrong production year")]
        public int ProductionYear { get; set; }

        [Required(ErrorMessage = "Max vehicle speed field is required")]
        [Range(1, 1000, ErrorMessage = "Speed must be a positive number less then 1000 kph")]
        public int MaxSpeed { get; set; }

        [Required(ErrorMessage = "Exterior color field is required")]
        [MinLength(1, ErrorMessage = "Exterior color must be valid")]
        [MaxLength(100, ErrorMessage = "Exterior color must be valid")]
        public string ExteriorColor { get; set; } = null!;

        [Required(ErrorMessage = "Interior color field is required")]
        [MinLength(1, ErrorMessage = "Interior color must be valid")]
        [MaxLength(100, ErrorMessage = "Interior color must be valid")]
        public string InteriorColor { get; set; } = null!;

        [Required(ErrorMessage = "Drivetrain field is required")]
        [Range(0, 100, ErrorMessage = "Drivetrain field is required")]
        public Drivetrain Drivetrain { get; set; } = Drivetrain.Empty;

        [Required(ErrorMessage = "Fuel type field is required")]
        [Range(0, 100, ErrorMessage = "Fuel type field is required")]
        public FuelType FuelType { get; set; } = FuelType.Empty;

        [Required(ErrorMessage = "Transmission field is required")]
        [Range(0, 100, ErrorMessage = "Transmission field is required")]
        public Transmission Transmission { get; set; } = Transmission.Empty;

        [Required(ErrorMessage = "Engine field is required")]
        [Range(0, 100, ErrorMessage = "Engine field is required")]
        public Engine Engine { get; set; } = Engine.Empty;

        [Required(ErrorMessage = "VIN field is required")]
        [MinLength(17, ErrorMessage = "VIN must contains 17 symbols")]
        [MaxLength(17, ErrorMessage = "VIN must contains 17 symbols")]
        public string VIN { get; set; } = null!;

        public Location Location { get; set; } = new Location() 
        { 
            Address = null!,
            Latitude = null!, 
            Longitude = null!
        };
    }
}
