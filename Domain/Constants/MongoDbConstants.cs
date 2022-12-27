namespace CarSharingApp.Domain.Constants
{
    public class MongoDbConstants
    {
        public const string LOCAL_CONNECTION_STRING = "mongodb://127.0.0.1:27017";
        public const string CLOUD_CONNECTION_STRING = "mongodb+srv://Its_Glebyshka:15042003april@carsharingcluster.fezrpoe.mongodb.net/?retryWrites=true&w=majority";

        public const string DATABASE_NAME = "CarSharing";

        public const string ADMINS_COLLECTION_NAME = "Admins";
        public const string CUSTOMERS_COLLECTION_NAME = "Customers";
        public const string PAYMENTS_COLLECTION_NAME = "Payments";
        public const string RENTALS_COLLECTION_NAME = "Rentals";
        public const string REVIEWS_COLLECTION_NAME = "Reviews";
        public const string VEHICLES_COLLECTION_NAME = "Vehicles";
    }
}
