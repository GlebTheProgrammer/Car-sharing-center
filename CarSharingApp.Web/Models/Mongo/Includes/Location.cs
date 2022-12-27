using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.Mongo.Includes
{
    public class Location
    {
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "Coordinates must be set on the map")] public string Latitude { get; set; } = null!;
        public string Longitude { get; set; } = null!;
    }
}
