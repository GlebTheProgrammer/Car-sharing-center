namespace CarSharingApp.Application.Contracts.Authorization
{
    public record AuthorizationRequest(string EmailOrLogin, string Password);
}
