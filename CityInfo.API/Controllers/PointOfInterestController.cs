using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointOfInterestController : ControllerBase
    {


        [HttpGet]
        public ActionResult<ICollection<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var result = CitiesDataStore.Current.GetCity(cityId)?.PointsOfInterest;
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int id)
        {
            var result = CitiesDataStore.Current.GetPointOfInterest(cityId, id);
            return (result != null) ? Ok(result) : NotFound();
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreationDto)
        {
            var city = CitiesDataStore.Current.GetCity(cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var pointOfInterest = new PointOfInterestDto()
            {
                Id = ++pointOfInterestId,
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
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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

        //test commentnnnn
        [HttpPatch("{pointofinterestid}")]
        public ActionResult PatchPointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
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
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            city.PointsOfInterest.Remove(pointOfInterest);
            return NoContent();
        }
    }

    

}
