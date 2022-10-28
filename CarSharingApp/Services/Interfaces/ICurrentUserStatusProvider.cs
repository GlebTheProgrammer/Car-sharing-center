using CarSharingApp.Services.Includes;

namespace CarSharingApp.Services.Interfaces
{
    public interface ICurrentUserStatusProvider
    {
        public int? GetUserId();
        public UserRole GetUserRole();
        public void SetUserCredentials(int? userId, UserRole userRole);
    }
}
