using AutoMapper;
using CarSharingApp.Models.Mongo;
using CarSharingApp.Models.MongoView;
using CarSharingApp.OptionsSetup;
using CarSharingApp.Payment;
using CarSharingApp.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;

namespace CarSharingApp.Repository.MongoDbRepository
{
    public class MongoDbService : IMongoDbRepository
    {
        private const string atlasConnectionString = "mongodb+srv://Its_Glebyshka:15042003april@carsharingcluster.fezrpoe.mongodb.net/?retryWrites=true&w=majority";

        private readonly IMongoCollection<Admin> _adminsCollection;
        private readonly IMongoCollection<Credentials> _credentialsCollection;
        private readonly IMongoCollection<Customer> _customersCollection;
        private readonly IMongoCollection<Rating> _ratingsCollection;
        private readonly IMongoCollection<Rental> _rentalsCollection;
        private readonly IMongoCollection<Specifications> _specificationsCollection;
        private readonly IMongoCollection<Transaction> _transactionsCollection;
        private readonly IMongoCollection<Vehicle> _vehiclesCollection;
        private readonly IMongoCollection<Account> _accountsCollection;

        private readonly IMapper _mapper;

        public MongoDbService(IOptions<CarSharingDatabaseSettings> carSharingDatabaseSettings, IMapper mapper)
        {
            carSharingDatabaseSettings.Value.ConnectionString = atlasConnectionString;
            var mongoClient = new MongoClient(carSharingDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(carSharingDatabaseSettings.Value.DatabaseName);

            _accountsCollection = mongoDatabase.GetCollection<Account>(carSharingDatabaseSettings.Value.AccountsCollectionName);
            _adminsCollection = mongoDatabase.GetCollection<Admin>(carSharingDatabaseSettings.Value.AdminsCollectionName);
            _credentialsCollection = mongoDatabase.GetCollection<Credentials>(carSharingDatabaseSettings.Value.CredentialsCollectionName);
            _customersCollection = mongoDatabase.GetCollection<Customer>(carSharingDatabaseSettings.Value.CustomersCollectionName);
            _ratingsCollection = mongoDatabase.GetCollection<Rating>(carSharingDatabaseSettings.Value.RatingsCollectionName);
            _rentalsCollection = mongoDatabase.GetCollection<Rental>(carSharingDatabaseSettings.Value.RentalsCollectionName);
            _specificationsCollection = mongoDatabase.GetCollection<Specifications>(carSharingDatabaseSettings.Value.SpecificationsCollectionName);
            _transactionsCollection = mongoDatabase.GetCollection<Transaction>(carSharingDatabaseSettings.Value.TransactionsCollectionName);
            _vehiclesCollection = mongoDatabase.GetCollection<Vehicle>(carSharingDatabaseSettings.Value.VehiclesCollectionName);

            _mapper = mapper;
        }

        public async Task AddNewVehicleAsync(VehicleCreateModel vehicleCreateModel, HttpContext httpContext)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(httpContext.Session.GetString("JWToken"));
            string activeUserId = jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;

            Vehicle vehicle = _mapper.Map<Vehicle>(vehicleCreateModel);
            vehicle.OwnerId = activeUserId;
            await _vehiclesCollection.InsertOneAsync(vehicle);

            Specifications specifications = _mapper.Map<Specifications>(vehicleCreateModel);
            specifications.VehicleId = vehicle.Id;
            await _specificationsCollection.InsertOneAsync(specifications);

            Rating rating = new Rating()
            {
                VehicleId = vehicle.Id,
            };
            await _ratingsCollection.InsertOneAsync(rating);
        }

        public async Task<Credentials> GetCredetnialsByUserId(string ownerId)
        {
            return await _credentialsCollection.Find(c => c.UserId == ownerId).FirstOrDefaultAsync();
        }

        public async Task RegisterNewCustomer(CustomerRegisterModel newCustomer)
        {
            Customer customer = _mapper.Map<Customer>(newCustomer);
            await _customersCollection.InsertOneAsync(customer);

            Credentials newCustomerCredentials = _mapper.Map<Credentials>(newCustomer);
            newCustomerCredentials.UserId = customer.Id;
            newCustomerCredentials.Password = PasswordHashGeneratorService.GenerateNewPasswordHash(newCustomer.Password);
            await _credentialsCollection.InsertOneAsync(newCustomerCredentials);

            Account newCustomerAccount = new Account();
            newCustomerAccount.CustomerId = customer.Id;
            await _accountsCollection.InsertOneAsync(newCustomerAccount);
        }

        public async Task<Customer> TrySignIn(UserSignInModel signInCredentials)
        {
            signInCredentials.Password = PasswordHashGeneratorService.GenerateNewPasswordHash(signInCredentials.Password);

            Credentials signedUserCredentials = await _credentialsCollection.Find(c => c.Email == signInCredentials.Email && c.Password == signInCredentials.Password).FirstOrDefaultAsync();

            if (signedUserCredentials == null)
                return null;
            else
                return await _customersCollection.Find(customer => customer.Id == signedUserCredentials.UserId).FirstOrDefaultAsync();
        }

        public async Task<List<VehicleCatalogModel>> GetAllPublishedAndNotOrderedVehicles_CatalogRepresentation()
        {
            List<Vehicle> vehicles = await _vehiclesCollection.Find(vehicle => !vehicle.IsOrdered && vehicle.IsPublished).ToListAsync();

            return _mapper.Map<List<VehicleCatalogModel>>(vehicles);
        }

        public async Task<List<Vehicle>> GetPublishedAndNotOrderedVehicles()
        {
            return await _vehiclesCollection.Find(vehicle => !vehicle.IsOrdered && vehicle.IsPublished).ToListAsync();
        }

        public async Task<List<RentalAccountModel>> GetCustomerActiveRentals_AccountRepresentation(string customerId)
        {
            List<Rental> activeRentals = await _rentalsCollection.Find(rental => rental.CustomerId == customerId && rental.IsActive).ToListAsync();

            return _mapper.Map<List<RentalAccountModel>>(activeRentals);
        }

        public async Task<List<VehicleAccountModel>> GetCustomerVehicles_AccountRepresentation(string customerId)
        {
            List<Vehicle> vehicles = await _vehiclesCollection.Find(vehicle => vehicle.OwnerId == customerId).ToListAsync();

            return _mapper.Map<List<VehicleAccountModel>>(vehicles);
        }

        public async Task<CustomerAccountModel> GetCustomerInformation_AccountRepresentation(string customerId)
        {
            Customer customer = await _customersCollection.Find(customer => customer.Id == customerId).FirstOrDefaultAsync();
            Account customerAccount = await _accountsCollection.Find(account => account.CustomerId == customerId).FirstOrDefaultAsync();
            Credentials credentials = await _credentialsCollection.Find(credentials => credentials.UserId == customerId).FirstOrDefaultAsync();


            CustomerAccountModel accountInformation = _mapper.Map<CustomerAccountModel>(customer);
            _mapper.Map(customerAccount, accountInformation);
            _mapper.Map(credentials, accountInformation);

            return accountInformation;
        }

        public async Task PublishVehicleInTheCatalog(string vehicleId)
        {
            Vehicle updatedVehicle = await _vehiclesCollection.Find(vehicle => vehicle.Id == vehicleId).FirstOrDefaultAsync();
            updatedVehicle.IsPublished = true;

            await _vehiclesCollection.ReplaceOneAsync(vehicle => vehicle.Id == vehicleId, updatedVehicle);
        }

        public async Task HideVehicleFromTheCatalog(string vehicleId)
        {
            Vehicle updatedVehicle = await _vehiclesCollection.Find(vehicle => vehicle.Id == vehicleId).FirstOrDefaultAsync();
            updatedVehicle.IsPublished = false;

            await _vehiclesCollection.ReplaceOneAsync(vehicle => vehicle.Id == vehicleId, updatedVehicle);
        }

        public async Task DeleteVehicle(string vehicleId)
        {
            await _vehiclesCollection.DeleteOneAsync(vehicle => vehicle.Id == vehicleId);
            await _specificationsCollection.DeleteOneAsync(specifications => specifications.VehicleId == vehicleId);
            await _ratingsCollection.DeleteOneAsync(rating => rating.VehicleId == vehicleId);

            // Dont delete rentals and transactions connected with deleted vehicle
        }

        public async Task<Rental> GetRentalInformation(string rentalId)
        {
            return await _rentalsCollection.Find(rental => rental.Id == rentalId).FirstOrDefaultAsync();
        }

        public async Task SubmitCustomerRating(string vehicleId, RatingSubmitModel submittedRating)
        {
            Rating vehicleRating = await _ratingsCollection.Find(rating => rating.VehicleId == vehicleId).FirstOrDefaultAsync();

            Rating newRating = RatingCalculationProviderService.CalculateNewRating(vehicleRating, submittedRating);

            await _ratingsCollection.ReplaceOneAsync(rating => rating.Id == vehicleRating.Id, newRating);
        }

        public async Task FinishActiveRental(string rentalId)
        {
            Rental rental = await _rentalsCollection.Find(r => r.Id == rentalId).FirstOrDefaultAsync();

            rental.ReturnDate = DateTime.Now;
            rental.IsActive = false;

            await _rentalsCollection.ReplaceOneAsync(r => r.Id == rentalId, rental);

            Vehicle rentedVehicle = await _vehiclesCollection.Find(v => v.Id == rental.RentedVehicleId).FirstOrDefaultAsync();
            rentedVehicle.IsOrdered = false;

            rentedVehicle.TimesOrdered += 1;
            rentedVehicle.LastTimeOrdered = rental.ReturnDate;

            await _vehiclesCollection.ReplaceOneAsync(v => v.Id == rentedVehicle.Id, rentedVehicle);
        }

        public async Task<Vehicle> GetVehicleById(string vehicleId)
        {
            return await _vehiclesCollection.Find(v => v.Id == vehicleId).FirstOrDefaultAsync();
        }

        public async Task<CustomerProfileModel> GetCustomerProfile(string customerId, string vehicleId)
        {
            Customer customer = await _customersCollection.Find(c => c.Id == customerId).FirstOrDefaultAsync();
            Account customerAccount = await _accountsCollection.Find(a => a.CustomerId == customerId).FirstOrDefaultAsync();
            Credentials customerCredentials = await _credentialsCollection.Find(c => c.UserId == customerId).FirstOrDefaultAsync();

            CustomerProfileModel customerProfile = _mapper.Map<CustomerProfileModel>(customer);
            _mapper.Map(customerAccount, customerProfile);
            _mapper.Map(customerCredentials, customerProfile);
            customerProfile.VehicleId = vehicleId;

            return customerProfile;
        }

        public async Task<Customer> GetCustomerById(string customerId)
        {
            return await _customersCollection.Find(c => c.Id == customerId).FirstOrDefaultAsync();
        }

        public async Task<CustomerEditModel> GetCustomer_EditRepresentation(string customerId)
        {
            Customer customer = await _customersCollection.Find(c => c.Id == customerId).FirstOrDefaultAsync();
            Account customerAccount = await _accountsCollection.Find(a => a.CustomerId == customerId).FirstOrDefaultAsync();
            Credentials customerCredentials = await _credentialsCollection.Find(c => c.UserId == customerId).FirstOrDefaultAsync();

            CustomerEditModel customerEditModel = _mapper.Map<CustomerEditModel>(customer);
            _mapper.Map(customerAccount, customerEditModel);
            _mapper.Map(customerCredentials, customerEditModel);

            return customerEditModel;
        }

        public async Task EditCustomerData(CustomerEditModel customerEditModel)
        {
            Customer customer = await _customersCollection.Find(c => c.Id == customerEditModel.CustomerId).FirstOrDefaultAsync();
            Account customerAccount = await _accountsCollection.Find(a => a.CustomerId == customerEditModel.CustomerId).FirstOrDefaultAsync();
            Credentials customerCredentials = await _credentialsCollection.Find(c => c.UserId == customerEditModel.CustomerId).FirstOrDefaultAsync();

            _mapper.Map(customerEditModel, customer);
            _mapper.Map(customerEditModel, customerAccount);
            _mapper.Map(customerEditModel, customerCredentials);

            await _customersCollection.ReplaceOneAsync(c => c.Id == customer.Id, customer);
            await _accountsCollection.ReplaceOneAsync(a => a.Id == customerAccount.Id, customerAccount);
            await _credentialsCollection.ReplaceOneAsync(c => c.Id == customerCredentials.Id, customerCredentials);
        }

        public async Task ChangePassword(string userId, string newPassword)
        {
            Credentials credentials = await _credentialsCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            credentials.Password = PasswordHashGeneratorService.GenerateNewPasswordHash(newPassword);

            await _credentialsCollection.ReplaceOneAsync(c => c.Id == credentials.Id, credentials);
        }

        public async Task<VehicleEditModel> GetVehicle_EditRepresentation(string vehicleId)
        {
            Vehicle vehicle = await _vehiclesCollection.Find(v => v.Id == vehicleId).FirstOrDefaultAsync();
            Specifications vehicleSpecifications = await _specificationsCollection.Find(s => s.VehicleId == vehicleId).FirstOrDefaultAsync();

            VehicleEditModel vehicleEditModel = _mapper.Map<VehicleEditModel>(vehicle);
            _mapper.Map(vehicleSpecifications, vehicleEditModel);

            return vehicleEditModel;
        }

        public async Task EditVehicleData(VehicleEditModel vehicleEditModel)
        {
            Vehicle vehicle = await _vehiclesCollection.Find(v => v.Id == vehicleEditModel.VehicleId).FirstOrDefaultAsync();
            Specifications vehicleSpecifications = await _specificationsCollection.Find(s => s.VehicleId == vehicleEditModel.VehicleId).FirstOrDefaultAsync();

            _mapper.Map(vehicleEditModel, vehicle);
            _mapper.Map(vehicleEditModel, vehicleSpecifications);

            await _vehiclesCollection.ReplaceOneAsync(v => v.Id == vehicle.Id, vehicle);
            await _specificationsCollection.ReplaceOneAsync(s => s.Id == vehicleSpecifications.Id, vehicleSpecifications);
        }

        public async Task<VehicleInformationModel> GetVehicleInformation(string vehicleId)
        {
            Vehicle vehicle = await _vehiclesCollection.Find(v => v.Id == vehicleId).FirstOrDefaultAsync();
            Customer vehicleOwner = await _customersCollection.Find(c => c.Id == vehicle.OwnerId).FirstOrDefaultAsync();
            Specifications vehicleSpecifications = await _specificationsCollection.Find(s => s.VehicleId == vehicleId).FirstOrDefaultAsync();
            Rating vehicleRating = await _ratingsCollection.Find(r => r.VehicleId == vehicleId).FirstOrDefaultAsync();

            VehicleInformationModel vehicleInformationModel = _mapper.Map<VehicleInformationModel>(vehicle);
            _mapper.Map(vehicleSpecifications, vehicleInformationModel);
            vehicleInformationModel.Rating = _mapper.Map<RatingInformationModel>(vehicleRating);
            vehicleInformationModel.OwnerUsername = vehicleOwner.Username;

            return vehicleInformationModel;
        }

        public async Task StartNewRental(string customerId, PaymentUrlModel paymentInfo)
        {
            Customer customer = await _customersCollection.Find(c => c.Id == customerId).FirstOrDefaultAsync();
            Vehicle rentedVehicle = await _vehiclesCollection.Find(v => v.Id == paymentInfo.VehicleId).FirstOrDefaultAsync();
            Customer vehicleOwner = await _customersCollection.Find(c => c.Id == rentedVehicle.OwnerId).FirstOrDefaultAsync();

            DateTime rentalStartDate = new DateTime(year: DateTime.Now.Year, month: int.Parse(paymentInfo.StartMonth), day: int.Parse(paymentInfo.StartDay), hour: int.Parse(paymentInfo.StartHour), 0, 0);

            //Rental rental = new Rental()
            //{
            //    CustomerId = customer.Id,
            //    RentedVehicleId = rentedVehicle.Id,
            //    RentedVehicleName = rentedVehicle.Name,
            //    RentalDate = rentalStartDate,
            //    ReturnDate = rentalStartDate.AddDays(paymentInfo.) 
            //    IsActive = true
            //}

            //Transaction transaction = new Transaction()
            //{
            //    Amount = paymentInfo.Amount,
            //    RentalId
            //}

        }
    }
}
