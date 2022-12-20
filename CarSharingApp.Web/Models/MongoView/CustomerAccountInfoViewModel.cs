namespace CarSharingApp.Models.MongoView
{
    public class CustomerAccountInfoViewModel
    {
        public CustomerAccountModel CustomerAccountInformation { get; set; } = null!;

        public int CustomerVehiclesCount { get; set; }
        public int CustomerPublishedVehiclesCount { get; set; }
        public int CustomerActiveRentalsCount { get; set; }

        public List<RentalAccountModel> ActiveRentals { get; set; } = null!;
        public List<VehicleAccountModel> CustomerVehicles { get; set; } = null!;
    }
}
