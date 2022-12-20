using CarSharingApp.Models.Mongo.Includes;

namespace CarSharingApp.Models.MongoView
{
    public class VehicleCatalogModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string BriefDescription { get; set; } = null!;
        public Tariff Tariff { get; set; } = new Tariff();
        public string Image { get; set; } = null!;

        public DateTime? LastTimeOrdered { get; set; } = null!;
        public int TimesOrdered { get; set; }
    }
}
