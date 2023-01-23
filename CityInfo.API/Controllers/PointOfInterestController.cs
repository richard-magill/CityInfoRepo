using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointOfInterestController : ControllerBase
    {
        private readonly ILogger<PointOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointOfInterestController(ILogger<PointOfInterestController> logger, 
            IMailService mailService,  
            ICityInfoRepository cityInfoRepository, 
            IMapper mapper)
        {
            _logger = logger;
            _mailService = mailService; 
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;   
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            var result = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
            return  Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(result));
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int id)
        {
            var result = await _cityInfoRepository.GetPointOfInterestAsync(cityId, id);
            return (result != null) ? Ok(_mapper.Map<PointOfInterestDto>(result)) : NotFound();
        }

        [HttpPost(Name = "AddPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreationDto)
        {
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                _logger.LogWarning($"this city id {cityId} was not found");
                return NotFound();
            }

            var pointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterestForCreationDto);
            await _cityInfoRepository.AddPointOfInterestAsync(cityId, pointOfInterest); 
            await _cityInfoRepository.SaveChangesAsync();  
            
            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(pointOfInterest);

            return Ok(createdPointOfInterestToReturn);



        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                _logger.LogWarning($"this city id {cityId} was not found");
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterestForUpdateDto, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PatchPointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                _logger.LogWarning($"this city id {cityId} was not found");
                return NotFound();
            }


            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!TryValidateModel(ModelState))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var cityExists = await _cityInfoRepository.CityExistsAsync(cityId);
            if (!cityExists)
            {
                _logger.LogWarning($"this city id {cityId} was not found");
                return NotFound();
            }
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);

            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            _mailService.Send("Deleted POI", $"PointOfInterest was deleted: {pointOfInterestEntity.Name}");
            return NoContent();
        }
    }

    

}
