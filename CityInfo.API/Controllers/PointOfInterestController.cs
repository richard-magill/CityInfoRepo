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
        private readonly CitiesDataStore? _cityDataStore;
        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, CitiesDataStore cityDataStore)
        {
            _logger = logger;
            _mailService = mailService; 
            _cityDataStore = cityDataStore; 
        }

        [HttpGet]
        public ActionResult<ICollection<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var result = _cityDataStore?.GetCity(cityId)?.PointsOfInterest;
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int id)
        {
            var result = _cityDataStore?.GetPointOfInterest(cityId, id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreationDto)
        {
            var city = _cityDataStore?.GetCity(cityId);
            if (city == null)
            {
                _logger.LogWarning($"this city id {cityId} was not found");
                return NotFound();
            }
            var pointOfInterestId = _cityDataStore?.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            if (pointOfInterestId == null)
            {
                return NotFound();
            }
            var pointOfInterest = new PointOfInterestDto()
            {
                Id = (int)(++pointOfInterestId),
                Name = pointOfInterestForCreationDto.Name,
                Description = pointOfInterestForCreationDto.Description
            };
            city.PointsOfInterest.Add(pointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    id = pointOfInterest.Id
                }, pointOfInterest);
        }

        [HttpPut("{pointofinterestid}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterestForUpdateDto)
        {
            var city = _cityDataStore?.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            pointOfInterest.Name = pointOfInterestForUpdateDto.Name;
            pointOfInterest.Description = pointOfInterestForUpdateDto.Description;
            return NoContent();
        }


        [HttpPatch("{pointofinterestid}")]
        public ActionResult PatchPointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _cityDataStore?.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!TryValidateModel(ModelState))
            {
                return BadRequest(ModelState);
            }

            pointOfInterest.Name = pointOfInterestToPatch.Name;
            pointOfInterest.Description = pointOfInterestToPatch.Description;
            return NoContent();

        }

        [HttpDelete("{pointofinterestid}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointofinterestid)
        {
            var city = _cityDataStore?.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointofinterestid);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            city.PointsOfInterest.Remove(pointOfInterest);
            _mailService.Send("Deleted POI", $"PointOfInterest was deleted: {pointOfInterest.Name}");
            return NoContent();
        }
    }

    

}
