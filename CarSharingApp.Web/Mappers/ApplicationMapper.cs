using AutoMapper;
using CarSharingApp.Models.Mongo;
using CarSharingApp.Models.MongoView;

namespace CarSharingApp.Mappers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // From -> Into
            CreateMap<VehicleCreateModel, Specifications>();
            CreateMap<VehicleCreateModel, Vehicle>();
            CreateMap<CustomerRegisterModel, Customer>();
            CreateMap<CustomerRegisterModel, Credentials>();
            CreateMap<Vehicle, VehicleCatalogModel>();
            CreateMap<Rental, RentalAccountModel>();
            CreateMap<Vehicle, VehicleAccountModel>();
            CreateMap<Customer, CustomerAccountModel>();
            CreateMap<Credentials, CustomerAccountModel>();
            CreateMap<Account, CustomerAccountModel>();
            CreateMap<Customer, CustomerProfileModel>();
            CreateMap<Account, CustomerProfileModel>();
            CreateMap<Credentials, CustomerProfileModel>();
            CreateMap<Customer, CustomerEditModel>();
            CreateMap<Account, CustomerEditModel>();
            CreateMap<Credentials, CustomerEditModel>();
            CreateMap<Vehicle, VehicleEditModel>();
            CreateMap<Specifications, VehicleEditModel>();
            CreateMap<Vehicle, VehicleInformationModel>();
            CreateMap<Specifications, VehicleInformationModel>();
            CreateMap<Rating, RatingInformationModel>();
            CreateMap<CustomerEditModel, Customer>();
            CreateMap<CustomerEditModel, Account>();
            CreateMap<CustomerEditModel, Credentials>();
            CreateMap<VehicleEditModel, Vehicle>();
            CreateMap<VehicleEditModel, Specifications>();
            CreateMap<Vehicle, VehicleHomeModel>();
        }
    }
}
