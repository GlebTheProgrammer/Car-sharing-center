using CarSharingApp.Services.Includes;

namespace CarSharingApp.Services.Interfaces
{
    public interface ICurrentUserStatusProvider
    {
        public int? GetUserId();
        public UserRole GetUserRole();
        public void SetUserCredentials(int? userId, UserRole userRole);

        public bool HasUserSignedIn();
        public bool HasUserLoggedOut();
        public bool HasTriedToGetUnauthorizedAccess();
        public bool HasChangedAccountData();
        public bool HasChangedPasswordData();

        public void ChangeSignedInState(bool state);
        public void ChangeLoggedOutState(bool state);
        public void ChangeUnauthorizedAccessState(bool state);
        public void ChangeAccountDataHasChangedState(bool state);
        public void ChangePasswordDataHasChangedState(bool state);
    }
}
