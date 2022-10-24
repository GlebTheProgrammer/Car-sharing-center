using AutoMapper;
using CarSharingApp.Models.VehicleData;

namespace CarSharingApp.Mappers
{
    public class VehiclesMapper : Profile
    {
        public VehiclesMapper()
        {
            // Add models mapping here

            CreateMap<VehicleShareModel, VehicleModel>();
        }
    }
}
