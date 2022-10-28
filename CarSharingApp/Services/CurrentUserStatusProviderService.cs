using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;

namespace CarSharingApp.Services
{
    public class CurrentUserStatusProviderService : ICurrentUserStatusProvider
    {
        private UserRole UserRole { get; set; } = UserRole.Unauthorized;
        private int? UserId { get; set; } = null;

        public int? GetUserId()
        {
            return UserId;
        }

        public UserRole GetUserRole()
        {
            return UserRole;
        }

        public void SetUserCredentials(int? userId, UserRole userRole)
        {
            UserId = userId;
            UserRole = userRole;
        }
    }
}
