using AutoMapper;
using CarSharingApp.Models.ClientData;
using CarSharingApp.Models.RatingData;
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
            CreateMap<VehicleModel, VehicleInformationViewModel>();
            CreateMap<RatingModel, VehicleRatingViewModel>();
        }
    }
}
