using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _dataStore;
        public CitiesController(CitiesDataStore dataStore)
        {
            _dataStore = dataStore;
        }
    
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return _dataStore.Cities;
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var result = _dataStore.GetCity(id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost]
        public ActionResult<CityDto> CreateCity([FromBody] CityForCreationDto dto)
        {
            var city = _dataStore.AddCity(dto);
            return Ok(city);
        }
    }
}
