using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }
    
        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<CityDto>>> GetCities([FromQuery] string? name, string? searchQuery, int pageNumber=1,int  pageSize= 10)
        {
            if( pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var (result, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(_mapper.Map<IEnumerable<CityDto>>(result));  
        }

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetCity(int cityId, bool includePointsOfInterest = true)
        {
            var result = await _cityInfoRepository.GetCityAsync(cityId, includePointsOfInterest);
            return (result != null) ? Ok(_mapper.Map<CityDto>(result)) : NotFound();
        }


    }
}
