namespace CarSharingApp.OptionsSetup
{
    public class CarSharingDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string AccountsCollectionName { get; set; } = null!;
        public string AdminsCollectionName { get; set; } = null!;
        public string CredentialsCollectionName { get; set; } = null!;
        public string CustomersCollectionName { get; set; } = null!;
        public string RatingsCollectionName { get; set; } = null!;
        public string RentalsCollectionName { get; set; } = null!;
        public string SpecificationsCollectionName { get; set; } = null!;
        public string TransactionsCollectionName { get; set; } = null!;
        public string VehiclesCollectionName { get; set; } = null!;
    }
}
