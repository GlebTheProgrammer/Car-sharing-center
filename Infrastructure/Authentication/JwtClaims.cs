namespace CarSharingApp.Infrastructure.Authentication
{
    public sealed class JwtClaims
    {
        public string Id { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
