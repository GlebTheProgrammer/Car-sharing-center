namespace CarSharingApp.Models.Mongo.Includes
{
    public class Location
    {
        public string Address { get; set; } = null!;
        public string Latitude { get; set; } = null!;
        public string Longitude { get; set; } = null!;
    }
}
