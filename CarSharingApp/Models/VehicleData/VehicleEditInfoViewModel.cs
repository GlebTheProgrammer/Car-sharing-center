using CarSharingApp.Models.VehicleData.Includes;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.VehicleData
{
    public class VehicleEditInfoViewModel
    {
        public int Id { get; set; }

        // Пользователь не может изменять эти поля
        public string Name { get; set; }
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

        // Пользователь может изменять эти поля
        public Location Location { get; set; }

        [Required(ErrorMessage = "Brief description field is required")]
        [MinLength(10, ErrorMessage = "Brief description can't be less then 10 symbols")]
        [MaxLength(40, ErrorMessage = "Brief description can't be longer then 40 symbols")]
        public string BriefDescription { get; set; }

        [Required(ErrorMessage = "Description field is required")]
        [MinLength(30, ErrorMessage = "Description can't be less then 30 symbols")]
        [MaxLength(255, ErrorMessage = "Brief description can't be longer then 255 symbols")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Tariff field is required")]
        public Tariff Tariff { get; set; }
    }
}
