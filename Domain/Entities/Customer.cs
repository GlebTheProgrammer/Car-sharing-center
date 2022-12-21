using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.Entities
{
    public sealed class Customer : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Address { get; private set; }
        public string PhoneNumber { get; private set; }
        public string DriverLicenseIdentifier { get; private set; }
        public string ProfileDescription { get; private set; }
        public string ProfileImage { get; private set; }
        public int Postcode { get; private set; }
        public int VehiclesOrdered { get; private set; }
        public int VehiclesShared { get; private set; }
        public bool IsAcceptedNewsSharing { get; private set; }
        public bool IsOnline { get; private set; }

        public CustomerCredentials? Credentials { get; private set; } // 1:1
        public List<Vehicle> Vehicles { get; private set; } = new(); // 1:many
        public List<Review> Reviews { get; private set; } = new(); // 1:many

        public Customer(Guid id,
            string firstName,
            string lastName,
            string country,
            string city,
            string address,
            string phoneNumer,
            string driverLicenseIdentifier,
            string profileDecription,
            string profileImage,
            int postcode,
            int vehiclesOrdered,
            int vehiclesShared,
            bool isAcceptedNewsSharing,
            bool isOnline) 
            : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            City = city;
            Address = address;
            PhoneNumber = phoneNumer;
            DriverLicenseIdentifier = driverLicenseIdentifier;
            ProfileDescription = profileDecription;
            ProfileImage = profileImage;
            Postcode = postcode;
            VehiclesOrdered = vehiclesOrdered;
            VehiclesShared = vehiclesShared;
            IsAcceptedNewsSharing = isAcceptedNewsSharing;
            IsOnline = isOnline;
        }
    }
}
