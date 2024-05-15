using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CityVehicleAPI.ModelsDTO
{
    public class CityDto
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public string CommonVehicle { get; set; }
    }
}
