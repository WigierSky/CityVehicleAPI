using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CityVehicleAPI.Models
{
    public class City
    {
        [Key]
        [SwaggerIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Population is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Population must be a non-negative number")]
        [DefaultValue(0)]
        public int Population { get; set; }
    }

}

