using AutoMapper;
using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Mappers
{
    public class VehiclesMapper : Profile
    {
        public VehiclesMapper()
        {
            // Add models mapping here

            // From -> Into
            CreateMap<VehicleShareModel, VehicleModel>();
            CreateMap<VehicleModel, VehicleViewModel>();
        }
    }
}
