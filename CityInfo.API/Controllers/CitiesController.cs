using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return CitiesDataStore.Current.Cities;
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var result = CitiesDataStore.Current.GetCity(id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost]
        public ActionResult<CityDto> CreateCity([FromBody] CityForCreationDto dto)
        {
            var city = CitiesDataStore.Current.AddCity(dto);
            return Ok(city);
        }
    }
}
