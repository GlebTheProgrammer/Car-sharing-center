using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.IdentityServer.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Usernane { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}
