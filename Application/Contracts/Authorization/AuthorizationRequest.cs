using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Application.Contracts.Authorization
{
    public sealed class AuthorizationRequest
    {
        [Required]
        public string EmailOrLogin { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
