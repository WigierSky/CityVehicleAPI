using CityVehicleAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CityVehicleAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
