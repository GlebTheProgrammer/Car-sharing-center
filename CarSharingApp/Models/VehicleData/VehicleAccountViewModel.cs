using CarSharingApp.Models.VehicleData.Includes;

namespace CarSharingApp.Models.VehicleData
{
    public class VehicleAccountViewModel
    {
        public int Id { get; set; }

        public DateTime PublishedTime { get; set; }

        public string Name { get; set; }
        public Tariff Tariff { get; set; }
        public int TimesOrdered { get; set; }

        // Активна ли техника или нет (меняется пользователем в личном кабинете)
        public bool IsPublished { get; set; }
        public bool IsOrdered { get; set; }
    }
}
