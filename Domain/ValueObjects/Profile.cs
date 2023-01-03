using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.ValidationErrors;
using ErrorOr;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Profile : ValueObject
    {
        public const string DefaultDescription = "No description yet";
        public const string DefaultImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTdmrjoiXGVFEcd1cX9Arb1itXTr2u8EKNpw&usqp=CAU";
        public const int MinDescriptionLength = 5;
        public const int MaxDescriptionLength = 100;
        public const int MinImageLength = 1;
        public const int MaxImageLength = 255;

        public string Description { get; private set; }
        public string Image { get; private set; }

        private Profile(
            string description,
            string image)
        {
            Description = description;
            Image = image;
        }

        public static ErrorOr<Profile> Create(
            string? description = null,
            string? image = null)
        {
            List<Error> errors = new();

            if (description is not null)
            {
                if (description.Length is > MaxDescriptionLength or < MinDescriptionLength)
                {
                    errors.Add(DomainErrors.Customer.InvalidProfileDescription);
                }
            }
            if (image is not null)
            {
                if (image.Length is > MaxImageLength or < MinImageLength)
                {
                    errors.Add(DomainErrors.Customer.InvalidImageLength);
                }
            }

            if (errors.Count > 0)
            {
                return errors;
            }

            return new Profile(
                description ?? DefaultDescription, 
                image ?? DefaultImage);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Description; 
            yield return Image;
        }
    }
}
