namespace CarSharingApp.Models.MongoView
{
    public class VehiclesHomeDataViewModel
    {
        public float[][] VehiclesLocation { get; set; } = null!;
        public List<VehicleHomeModel> Vehicles { get; set; } = null!;
    }
}
