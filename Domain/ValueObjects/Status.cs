using CarSharingApp.Domain.Primitives;
using ErrorOr;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Status : ValueObject
    {
        public bool IsConfirmedByAdmin { get; private set; }
        public bool IsPublished { get; private set; }
        public bool IsOrdered { get; private set; }

        public Status(
            bool isConfirmedByAdmin, 
            bool isPublished, 
            bool isOrdered)
        {
            IsConfirmedByAdmin = isConfirmedByAdmin;
            IsPublished = isPublished;
            IsOrdered = isOrdered;
        }

        public static ErrorOr<Status> Create(
            bool isConfirmedByAdmin = false,
            bool isPublished = false,
            bool isOrdered = false)
        {
            return new Status(
                isConfirmedByAdmin, 
                isPublished, 
                isOrdered);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return IsConfirmedByAdmin;
            yield return IsPublished;
            yield return IsOrdered;
        }
    }
}
