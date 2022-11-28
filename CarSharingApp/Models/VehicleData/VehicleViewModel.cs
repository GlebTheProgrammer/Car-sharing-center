using CarSharingApp.Models.VehicleData.Includes;

namespace CarSharingApp.Models.VehicleData
{
    public class VehicleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string BriefDescription { get; set; }
        public Tariff Tariff { get; set; }
        public string Image { get; set; }

        public DateTime LastTimeOrdered { get; set; }
        public int TimesOrdered { get; set; }
    }
}
