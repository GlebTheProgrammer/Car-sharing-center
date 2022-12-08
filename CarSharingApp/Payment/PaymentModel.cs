using CarSharingApp.Models.VehicleData.Includes;

namespace CarSharingApp.Payment
{
    public class PaymentModel
    {
        public int VehicleId { get; set; }

        public Tariff Tariff { get; set; }

        public string Amount { get; set; }
        public string VehicleName { get; set; }
        public string StartMonth { get; set; }
        public string EndMonth { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
       
    }
}
