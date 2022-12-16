using CarSharingApp.Models.Mongo.Enums;

namespace CarSharingApp.Models.MongoView
{
    public class CustomerProfileModel
    {
        public string VehicleId { get; set; }

        public string Username { get; set; } = null!;
        public Country Country { get; set; }
        public City City { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int VehiclesOrdered { get; set; }
        public int VehiclesShared { get; set; }
        public string AccountDescription { get; set; } = null!;
        public string UserImage { get; set; } = null!;
    }
}
