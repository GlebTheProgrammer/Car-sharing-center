using CarSharingApp.Models.Mongo;
using CarSharingApp.Models.MongoView;
using CarSharingApp.Payment;

namespace CarSharingApp.Repository.MongoDbRepository
{
    public interface IMongoDbRepository
    {
        public Task AddNewVehicleAsync(VehicleCreateModel newVehicle, HttpContext httpContext);
        public Task RegisterNewCustomer(CustomerRegisterModel newCustomer);
        public Task<Customer> TrySignIn(UserSignInModel signInCredentials);
        public Task<Credentials> GetCredetnialsByUserId(string ownerId);
        public Task<Vehicle> GetVehicleById(string vehicleId);
        public Task<Customer> GetCustomerById(string customerId);
        public Task<List<VehicleCatalogModel>> GetAllPublishedAndNotOrderedVehicles_CatalogRepresentation();
        public Task<List<Vehicle>> GetPublishedAndNotOrderedVehicles();
        public Task<List<RentalAccountModel>> GetCustomerActiveRentals_AccountRepresentation(string customerId);
        public Task<List<VehicleAccountModel>> GetCustomerVehicles_AccountRepresentation(string customerId);
        public Task<CustomerAccountModel> GetCustomerInformation_AccountRepresentation(string customerId);
        public Task PublishVehicleInTheCatalog(string vehicleId);
        public Task HideVehicleFromTheCatalog(string vehicleId);
        public Task DeleteVehicle(string vehicleId);
        public Task<Rental> GetRentalInformation(string rentalId);
        public Task SubmitCustomerRating(string vehicleId, RatingSubmitModel submittedRating);
        public Task FinishActiveRental(string rentalId);
        public Task<CustomerProfileModel> GetCustomerProfile(string customerId, string vehicleId);
        public Task<CustomerEditModel> GetCustomer_EditRepresentation(string customerId);
        public Task EditCustomerData(CustomerEditModel customerEditModel);
        public Task ChangePassword(string userId, string newPassword);
        public Task<VehicleEditModel> GetVehicle_EditRepresentation(string vehicleId);
        public Task EditVehicleData(VehicleEditModel vehicleEditModel);
        public Task<VehicleInformationModel> GetVehicleInformation(string vehicleId);
        public Task StartNewRental(string customerId, PaymentUrlModel paymentInfo);
    }
}
