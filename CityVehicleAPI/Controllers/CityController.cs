using AutoMapper;
using CityVehicleAPI.Context;
using CityVehicleAPI.Models;
using CityVehicleAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CityVehicleAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class CityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CityController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// POST: /city
        /// Adds a new city to the database
        /// </summary>
        /// <param name="city">The city object to add</param>
        /// <response code="201">Returns the newly created city</response>
        /// <response code="400">If the city is null or invalid</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Post city info")]
        public async Task<ActionResult<City>> PostCity([FromBody] City city)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCityDtoById), new { cityId = city.Id }, city);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// GET: /city
        /// Returns details about a randomly selected city from the database.
        /// </summary>
        /// <response code="200">Returns the information about the randomly selected city</response>
        /// <response code="404">If there are no cities in the database</response>
        /// <response code="500">If there was an unexpected server error</response>
        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get random city",
            Description = "Returns information about random city")]
        public async Task<ActionResult<CityDto>> RandomCity()
        {
            try
            {
                int count = await _context.Cities.CountAsync();
                int randomIndex = new Random().Next(1, count + 1);
                var randomCity = await _context.Cities.Where(c => c.Id == randomIndex).FirstOrDefaultAsync();
                if (randomCity == null)
                {
                    return NotFound("No cities in the database!");
                }

                var cityDto = _mapper.Map<CityDto>(randomCity);

                return Ok(cityDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// GET: /city/vehicle/{vehicleName}
        /// Returns a list of cities where the specified vehicle is commonly used
        /// </summary>
        /// <param name="vehicleName">The name of the vehicle</param>
        /// <response code="200">Returns the list of cities where the specified vehicle is commonly used</response>
        /// <response code="404">If there are no cities where the specified vehicle is commonly used</response>
        /// <response code="400">If the provided vehicle name is empty or null</response>
        /// <response code="500">If there was an unexpected server error</response>
        [HttpGet("vehicle/{vehicleName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get cities by vehicle",
            Description = "Returns information about cities where the specified vehicle is commonly used")]
        public async Task<ActionResult<List<City>>> GetCitiesByVehicle(string vehicleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(vehicleName))
                {
                    return BadRequest("Vehicle name cannot be empty or null.");
                }

                var cities = await _context.Cities
                .Where(c => _context.Vehicles.Any(v =>
                    v.VehicleName.ToLower() == vehicleName.ToLower() &&
                    c.Population >= v.MinPopulation &&
                    c.Population <= v.MaxPopulation))
                .ToListAsync();

                if (cities.Count != 0)
                    return Ok(cities);
                else
                    return NotFound("There is no city this vehicle is commonly used!");
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Returns information about cities with the specified name from the database
        /// </summary>
        /// <param name="cityName">The name of the city to search for</param>
        /// <response code="200">Returns the information about cities with the specified name</response>
        /// <response code="404">If there are no cities with the specified name in the database</response>
        /// <response code="400">If the city name provided is empty or null</response>
        /// <response code="500">If there was an unexpected server error</response>
        [HttpGet("cityByName/{cityName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get cities by name",
            Description = "Returns information about cities with the specified name from the database")]
        public async Task<ActionResult<List<CityDto>>> GetCitiesDtoByName(string cityName)
        {
            try
            {
                if (string.IsNullOrEmpty(cityName)) return BadRequest("City name can't be empty!");

                var cities = await _context.Cities
                    .Where(c => c.Name == cityName)
                    .ToListAsync();

                if (cities.Count == 0) return NotFound("Can't find any city with this name!");

                var cityList = _mapper.Map<List<CityDto>>(cities);

                return Ok(cityList);

            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// GET: /city/{cityId}
        /// Return a city by its ID
        /// </summary>
        /// <param name="cityId">The ID of the city to retrieve</param>
        /// <response code="200">Returns the requested city</response>
        /// <response code="404">If the city with the given ID was not found</response>    
        /// <response code="400">If the city ID is null or invalid</response>    
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get city by id",
            Description = "Returns information about city with the specified id from the database")]
        public async Task<ActionResult<CityDto>> GetCityDtoById(int cityId)
        {
            try
            {
                if (cityId == null) return BadRequest("Incorrect city id!");

                var city = await _context.Cities.FindAsync(cityId);

                if (city == null) return NotFound("Can't find city with this id!");

                var cityDto = _mapper.Map<CityDto>(city);

                return Ok(cityDto);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
