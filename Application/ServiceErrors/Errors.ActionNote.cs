using ErrorOr;

namespace CarSharingApp.Application.ServiceErrors
{
    public static partial class ApplicationErrors
    {
        public static class ActionNote
        {
            public static Error WrongType => Error.NotFound(
            code: "ActionNote.NotFound",
            description: "Note type you've provided does not exist.");
        }
    }
}
