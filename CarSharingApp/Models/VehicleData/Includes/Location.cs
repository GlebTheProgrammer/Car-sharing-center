using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CarSharingApp.Models.VehicleData.Includes
{
    public class Location
    {
        [Required(ErrorMessage = "Address field is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Coordinates must be set on the map")]
        [RegularExpression("^-?\\d*(\\.\\d+)?$", ErrorMessage = "Wrong coordinates were set. Please try again")]
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
