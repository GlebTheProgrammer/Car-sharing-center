using CarSharingApp.Models.Mongo.Includes;

namespace CarSharingApp.Models.MongoView
{
    public class VehicleAccountModel
    {
        public string Id { get; set; }

        public DateTime PublishedTime { get; set; }

        public string Name { get; set; } = null!;
        public Tariff Tariff { get; set; } = new Tariff();
        public int TimesOrdered { get; set; }

        public bool IsPublished { get; set; }
        public bool IsOrdered { get; set; }
    }
}
