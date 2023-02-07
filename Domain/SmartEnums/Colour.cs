using CarSharingApp.Domain.Primitives;

namespace CarSharingApp.Domain.SmartEnums
{
    public sealed class Colour : Enumeration<Colour>
    {
        public static readonly Colour Other = new(0, "Other");
        public static readonly Colour White = new(1, "White");
        public static readonly Colour Black = new(2, "Black");
        public static readonly Colour Gray = new(3, "Gray");
        public static readonly Colour Silver = new(4, "Silver");
        public static readonly Colour Blue = new(5, "Blue");
        public static readonly Colour Red = new(6, "Red");
        public static readonly Colour Brown = new(7, "Brown"); 
        public static readonly Colour Green = new(8, "Green");
        public static readonly Colour Orange = new(9, "Orange");
        public static readonly Colour Beige = new(10, "Beige");

        private Colour(int value, string name) : base(value, name)
        {
        }
    }
}
