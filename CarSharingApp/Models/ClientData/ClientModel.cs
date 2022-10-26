using CarSharingApp.Models.ClientData.Includes;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.ClientData
{
    public class ClientModel
    {
        public int Id { get; set; }
        public Role Role { get; set; }

        // This Fields come from ViewModel when register
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Country Country { get; set; }
        public BelarusCity BelarusCity { get; set; }
        public int Postcode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DriverLicenseNumber { get; set; }
        public bool IsAcceptedNewsSharing { get; set; }

    }
}
