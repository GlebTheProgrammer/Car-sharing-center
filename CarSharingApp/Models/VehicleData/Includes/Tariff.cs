using System.ComponentModel.DataAnnotations;

namespace CarSharingApp.Models.VehicleData.Includes
{
    public class Tariff
    {
        [Required(ErrorMessage = "Wrong hours tariff")]
        [Range(1, 100000, ErrorMessage = "Wrong hours tariff.")]
        public decimal TariffPerHour { get; set; }

        [Required(ErrorMessage = "Wrong daily tariff")]
        [Range(1, 100000, ErrorMessage = "Wrong daily tariff")]
        public decimal TariffPerDay { get; set; }
    }
}
