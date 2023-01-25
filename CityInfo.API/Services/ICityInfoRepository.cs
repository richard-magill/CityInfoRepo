using CityInfo.API.Entities;
using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        public Task<IEnumerable<City>> GetCitiesAsync();
        public Task<(IEnumerable<City>,PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        public Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        public Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        public Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointOfInterestId);

        public Task<bool> CityExistsAsync(int cityId);

        public Task AddPointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);
        public Task<bool> SaveChangesAsync();
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
