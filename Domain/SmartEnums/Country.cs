using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class Country : Enumeration<Country>
    {
        public static readonly Country Germany = new(1, nameof(Germany));
        public static readonly Country Latvia = new(2, nameof(Latvia));
        public static readonly Country USA = new(3, nameof(USA));
        public static readonly Country Netherlands = new(4, nameof(Netherlands));
        public static readonly Country Poland = new(5, nameof(Poland));
        public static readonly Country Cyprus = new(6, nameof(Cyprus));
        public static readonly Country Belarus = new(7, nameof(Belarus));
        public static readonly Country Ukrain = new(8, nameof(Ukrain));
        public static readonly Country Lithuania = new(9, nameof(Lithuania));

        private Country(int value, string name) 
            : base(value, name)
        {
        }
    }
}
