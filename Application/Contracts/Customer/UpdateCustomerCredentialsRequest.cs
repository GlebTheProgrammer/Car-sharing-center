namespace CarSharingApp.Application.Contracts.Customer
{
    public record UpdateCustomerCredentialsRequest(
        string Login,
        string Email
    );
}
