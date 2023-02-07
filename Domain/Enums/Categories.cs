using CarSharingApp.Domain.ValidationErrors;
using ErrorOr;

namespace CarSharingApp.Domain.Enums
{
    public static class FlagEnums
    {
        [Flags]
        public enum Categories
        {
            None = 0,
            CUV = 1,
            European = 2,
            Compact = 4,
            Electric = 8,
            Luxury = 16,
            Pickup = 32,
            Small = 64,
            Midsize = 128,
            Large = 256,
            Van = 512,
            Sedan = 1_024,
            Hatchback = 2_048,
            MPV = 4_096,
            Roadster = 8_192,
            Supercar = 16_384,
            Cabriolet = 32_768,
            Microcar = 65_536,
            Camper = 131_072,
            Limousine = 262_144,
            SUV = 524_288
        }

        public static ErrorOr<Categories> GetCategoriesFromList(List<string> categories)
        {
            List<string> values = GetPossibleValues<Categories>();
            Categories result = Categories.None;

            foreach (var category in categories)
            {
                if (!values.Contains(category))
                    return DomainErrors.Vehicle.InvalidCategories;

                result |= ParseEnum<Categories>(category);
            }

            return result;
        }

        public static List<string> GetListFromCategories(Categories categories)
        {
            List<string> values = GetPossibleValues<Categories>();
            List<string> result = new List<string>();

            foreach (var value in values)
            {
                bool isMatchEnumValue = (categories & ParseEnum<Categories>(value)) != 0;

                if (isMatchEnumValue)
                    result.Add(value);
            }

            return result;
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<string> GetPossibleValues<TEnum>()
            where TEnum : struct
        {
            Type type = typeof(TEnum);

            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            string[] enumPossibleStringValues = Enum.GetNames(type);

            return enumPossibleStringValues.ToList();
        }

        public static bool TwoCategoryMatch(Categories standart, Categories comparand)
        {
            // standart  101
            // comparand 001
            // result    001

            var comparison = standart & comparand;

            if (comparison != comparand)
                return false;

            return true;
        }
    }
}
