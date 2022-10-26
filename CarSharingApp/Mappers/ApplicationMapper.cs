using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Mappers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // Add models mapping here

            // From -> Into
            CreateMap<VehicleShareModel, VehicleModel>();
            CreateMap<VehicleModel, VehicleViewModel>();
            CreateMap<ClientRegistrationViewModel, ClientModel>();
        }
    }
}
