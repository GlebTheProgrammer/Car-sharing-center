using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;

namespace CarSharingApp.Services
{
    public class CurrentUserStatusProviderService : ICurrentUserStatusProvider
    {
        private UserRole UserRole { get; set; } = UserRole.Unauthorized;
        private int? UserId { get; set; } = null;
        public bool HasLoggedOut { get; set; } = false;
        public bool HasSignedIn { get; set; } = false;

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

        public bool HasUserSignedIn()
        {
            return HasSignedIn;
        }

        public bool HasUserLoggedOut()
        {
            return HasLoggedOut;
        }

        public void ChangeSignedInState(bool state)
        {
            HasSignedIn = state;
        }

        public void ChangeLoggedOutState(bool state)
        {
            HasLoggedOut = state;
        }
    }
}
