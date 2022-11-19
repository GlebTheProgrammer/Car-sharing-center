using CarSharingApp.Services.Includes;
using CarSharingApp.Services.Interfaces;

namespace CarSharingApp.Services
{
    public class CurrentUserStatusProviderService : ICurrentUserStatusProvider
    {
        private UserRole UserRole { get; set; } = UserRole.Unauthorized;
        private int? UserId { get; set; } = null;
        private bool HasLoggedOut { get; set; } = false;
        private bool HasSignedIn { get; set; } = false;
        private bool UnauthorizedAccess { get; set; } = false;
        public bool HasChangedUserAccountData { get; set; } = false;
        public bool HasChangedUserAccountPassword { get; set; } = false;

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

        public void ChangeUnauthorizedAccessState(bool state)
        {
            UnauthorizedAccess = state;
        }

        public bool HasTriedToGetUnauthorizedAccess()
        {
            return UnauthorizedAccess;
        }

        public bool HasChangedAccountData()
        {
            return HasChangedUserAccountData;
        }

        public void ChangeAccountDataHasChangedState(bool state)
        {
            HasChangedUserAccountData = state;
        }

        public bool HasChangedPasswordData()
        {
            return HasChangedUserAccountPassword;
        }

        public void ChangePasswordDataHasChangedState(bool state)
        {
            HasChangedUserAccountPassword = state;
        }
    }
}
