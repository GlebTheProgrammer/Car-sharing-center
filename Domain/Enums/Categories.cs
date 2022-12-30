namespace CarSharingApp.Domain.Enums
{
    [Flags]
    public enum Categories
    {
        SUV = 0,
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
        Limousine = 262_144
    }
}
