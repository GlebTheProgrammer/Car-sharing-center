using CarSharingApp.Models.Mongo.Enums;

namespace CarSharingApp.Models.MongoView
{
    public class CustomerAccountModel
    {
        public string? Id { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public Country Country { get; set; }
        public City City { get; set; }
        public int Postcode { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string DriverLicenseIdentifier { get; set; } = null!;
        public string Email { get; set; } = null!;

        public int VehiclesOrdered { get; set; }
        public int VehiclesShared { get; set; }
        public string AccountDescription { get; set; } = null!;
        public string CustomerImage { get; set; } = null!;

    }
}
