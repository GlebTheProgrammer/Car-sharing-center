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

        public void ChangeSignedInState(bool state);
        public void ChangeLoggedOutState(bool state);
        public void ChangeUnauthorizedAccessState(bool state);
    }
}
