using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CityVehicleAPI.Models
{
    public class Vehicle
    {
        [Key]
        [SwaggerIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Minimal population is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimal population must be a non-negative number")]
        [DefaultValue(0)]
        public int MinPopulation { get; set; }

        [Required(ErrorMessage = "Maximal population is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Maximal population must be a non-negative number")]
        [DefaultValue(0)]
        public int MaxPopulation { get; set; }

        [Required(ErrorMessage = "Vehicle name is required")]
        public string VehicleName { get; set; }
    }
}
