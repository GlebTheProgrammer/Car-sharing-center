using CarSharingApp.Models.ClientData.Includes;

namespace CarSharingApp.Models.ClientData
{
    public class ClientAccountViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public int Postcode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DriverLicenseNumber { get; set; }

        // История пользователя
        public int VehiclesOrdered { get; set; }
        public int VehiclesShared { get; set; }

        // Описание в личном кабинете пользователя
        public string AccountDescription { get; set; }
        public string UserImage { get; set; }
    }
}
