namespace CarSharingApp.Models.MongoView
{
    public class VehiclesCatalogDataViewModel
    {
        public List<VehicleCatalogModel> Vehicles { get; set; } = new List<VehicleCatalogModel>();
        public int NumberOfVehiclesToBeDisplayed { get; set; }
        public int NumberOfNotOrderedVehicles { get; set; }
        public int IndexOfFirstVehicleInTheList { get; set; }
        public int IndexOfLastVehicleInTheList { get; set; }
    }
}
