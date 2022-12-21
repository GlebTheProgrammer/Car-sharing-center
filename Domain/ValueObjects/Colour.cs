using CarSharingApp.Domain.Primitives;
using CarSharingApp.Domain.Exceptions;

namespace CarSharingApp.Domain.ValueObjects
{
    public sealed class Colour : ValueObject
    {
        private Colour()
        {
        }

        private Colour(string code)
        {
            Code = code;
        }

        public static Colour From(string code)
        {
            var colour = new Colour { Code = code };

            if (!SupportedColours.Contains(colour))
            {
                throw new UnsupportedVehicleColourException(code);
            }

            return colour;
        }

        public static Colour White => new("#FFFFFF");
        public static Colour Red => new("#FF5733");
        public static Colour Orange => new("#FFC300");
        public static Colour Yellow => new("#FFFF66");
        public static Colour Green => new("#CCFF99");
        public static Colour Blue => new("#6666FF");
        public static Colour Purple => new("#9966CC");
        public static Colour Grey => new("#999999");
        public static Colour Black => new("#000000");

        public string Code { get; private set; } = null!;

        public static implicit operator string(Colour colour)
        {
            return colour.ToString();
        }

        public static explicit operator Colour(string code)
        {
            return From(code);
        }

        public override string ToString()
        {
            return Code;
        }

        public static IEnumerable<Colour> SupportedColours
        {
            get
            {
                yield return White;
                yield return Red;
                yield return Orange;
                yield return Yellow;
                yield return Green;
                yield return Blue;
                yield return Purple;
                yield return Grey;
                yield return Black;
            }
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
        }
    }
}
