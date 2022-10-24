using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Views.CarSharing
{
    public class CarSharingDataViewModel
    {
        public List<VehicleViewModel> Vehicles { get; set; }
        public int NumberOfVehiclesDisplayed { get; set; }
        public int NumberOfVehicles { get; set; }
        public int StartVehiclesIndex { get; set; }
        public int EndVehiclesIndex { get; set; }
        public int PageNumber { get; set; }
    }
}
