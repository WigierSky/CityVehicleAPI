using AutoMapper;
using CityVehicleAPI.Models;
using CityVehicleAPI.ModelsDTO;

namespace CityVehicleAPI
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<City, CityDto>()
                .ForMember(dest => dest.CommonVehicle, opt => opt.MapFrom<VehicleResolver>());
        }
    }
}

