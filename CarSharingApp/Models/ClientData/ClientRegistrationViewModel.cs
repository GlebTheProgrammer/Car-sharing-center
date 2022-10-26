using CarSharingApp.Models.ClientData.Includes;
using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.ClientData
{
    public class ClientRegistrationViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(255, ErrorMessage = "Wrong First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(255, ErrorMessage = "Wrong Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(255, ErrorMessage = "Username must contains less then 30 symbols")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [Range(1, 100, ErrorMessage = "Country is required")]
        public Country Country { get; set; }

        [Required(ErrorMessage = "City is required")]
        [Range(1,100, ErrorMessage = "City is required")]
        public BelarusCity BelarusCity { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [DataType(DataType.PostalCode, ErrorMessage = "Wrong Post Code")]
        public int Postcode { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(255, ErrorMessage = "Wrong Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password did not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Valid Driver License Number is required")]
        [MaxLength(20, ErrorMessage = "Wrong Driver License Number")]
        public string DriverLicenseNumber { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must be 18+ years old and accept our Terms of Service")]
        public bool Is18YearsOld { get; set; }

        [Required]
        public bool IsAcceptedNewsSharing { get; set; }
    }
}
