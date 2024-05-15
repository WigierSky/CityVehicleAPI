using CityVehicleAPI.Context;
using CityVehicleAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

public static class SeedData
{
    /// <summary>
    /// Adding initial data to local database
    /// </summary>
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
        {
            if (context.Cities.Any() || context.Vehicles.Any())
            {
                return;
            }

            context.Cities.AddRange(
                new City { Name = "Wroclaw", Population = 600000 },
                new City { Name = "Poznan", Population = 800000 },
                new City { Name = "Ostrzeszow", Population = 2000 }
            );

            context.Vehicles.AddRange(
                new Vehicle { MinPopulation = 500000, MaxPopulation = 700000, VehicleName = "tram" },
                new Vehicle { MinPopulation = 1, MaxPopulation = 1000, VehicleName = "bicycle" },
                new Vehicle { MinPopulation = 1000, MaxPopulation = 100000, VehicleName = "car" },
                new Vehicle { MinPopulation = 100000, MaxPopulation = 500000, VehicleName = "Łódź" },
                new Vehicle { MinPopulation = 700000, MaxPopulation = 10000000, VehicleName = "plane" }
            );

            context.SaveChanges();
        }
    }
}
