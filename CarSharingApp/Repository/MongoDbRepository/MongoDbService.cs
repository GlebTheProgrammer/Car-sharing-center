using CarSharingApp.Models.Mongo;
using CarSharingApp.OptionsSetup;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarSharingApp.Repository.MongoDbRepository
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Admin> _adminsCollection;
        private readonly IMongoCollection<Credentials> _credentialsCollection;
        private readonly IMongoCollection<Customer> _customersCollection;
        private readonly IMongoCollection<Rating> _ratingsCollection;
        private readonly IMongoCollection<Rental> _rentalsCollection;
        private readonly IMongoCollection<Specifications> _specificationsCollection;
        private readonly IMongoCollection<Transaction> _transactionsCollection;
        private readonly IMongoCollection<Vehicle> _vehiclesCollection;

        public MongoDbService(IOptions<CarSharingDatabaseSettings> carSharingDatabaseSettings)
        {
            var mongoClient = new MongoClient(carSharingDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(carSharingDatabaseSettings.Value.DatabaseName);

            _adminsCollection = mongoDatabase.GetCollection<Admin>(carSharingDatabaseSettings.Value.AdminsCollectionName);
            _credentialsCollection = mongoDatabase.GetCollection<Credentials>(carSharingDatabaseSettings.Value.CredentialsCollectionName);
            _customersCollection = mongoDatabase.GetCollection<Customer>(carSharingDatabaseSettings.Value.CustomersCollectionName);
            _ratingsCollection = mongoDatabase.GetCollection<Rating>(carSharingDatabaseSettings.Value.RatingsCollectionName);
            _rentalsCollection = mongoDatabase.GetCollection<Rental>(carSharingDatabaseSettings.Value.RentalsCollectionName);
            _specificationsCollection = mongoDatabase.GetCollection<Specifications>(carSharingDatabaseSettings.Value.SpecificationsCollectionName);
            _transactionsCollection = mongoDatabase.GetCollection<Transaction>(carSharingDatabaseSettings.Value.TransactionsCollectionName);
            _vehiclesCollection = mongoDatabase.GetCollection<Vehicle>(carSharingDatabaseSettings.Value.VehiclesCollectionName);
        }


    }
}
