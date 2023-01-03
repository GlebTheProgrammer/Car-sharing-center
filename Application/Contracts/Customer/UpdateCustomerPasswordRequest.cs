namespace CarSharingApp.Application.Contracts.Customer
{
    public record UpdateCustomerPasswordRequest(
        string currentPassword,
        string newPassword
    );
}
