using AutoMapper;
using CityVehicleAPI.Models;
using CityVehicleAPI.ModelsDTO;
using CityVehicleAPI.Context;

namespace CityVehicleAPI
{
    public class VehicleResolver : IValueResolver<City, CityDto, string>
    {
        private readonly AppDbContext _context;

        public VehicleResolver(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets vehicle that is commonly used in city. 
        /// Selects first fitting vehicle.
        /// </summary>
        public string Resolve(City source, CityDto destination, string destMember, ResolutionContext context)
        {
            var vehicle = _context.Vehicles
                .Where(item => source.Population > item.MinPopulation && source.Population < item.MaxPopulation)
                .FirstOrDefault();

            return vehicle != null ? vehicle.VehicleName : "No vehicle meets the requirements!";
        }
    }

}
