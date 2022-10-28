using CarSharingApp.Models.VehicleData;
using CarSharingApp.Services.Includes;

namespace CarSharingApp.Views.CarSharing
{
    public class CarSharingDataViewModel
    {
        // Vehicles data
        public List<VehicleViewModel> Vehicles { get; set; }
        public int NumberOfVehiclesDisplayed { get; set; }
        public int NumberOfVehicles { get; set; }
        public int StartVehiclesIndex { get; set; }
        public int EndVehiclesIndex { get; set; }
    }
}
