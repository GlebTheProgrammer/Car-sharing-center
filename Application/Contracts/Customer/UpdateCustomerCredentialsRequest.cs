namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record UpdateCustomerCredentialsRequest(
        string Login,
        string Email
    );
}
