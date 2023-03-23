namespace CarSharingApp.Application.Contracts.Customer
{
    public sealed record UpdateCustomerPasswordRequest(
        string currentPassword,
        string newPassword
    );
}
