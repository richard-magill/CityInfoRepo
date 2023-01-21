using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities {get; set;}
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore() => Cities = new List<CityDto>
                {
                    new CityDto
                    {
                        Id = 1,
                        Name = "Antwerp",
                        Description = "Port city in Belgium",
                        PointsOfInterest =
                        {
                            new PointOfInterestDto
                            {
                                Id = 1,
                                Name="Cathedral",
                                Description="Big Church"
                            },
                            new PointOfInterestDto
                            {
                                Id = 2,
                                Name="Port",
                                Description="Docks and such"
                            }
                        }
                    },
                    new CityDto
                    {
                        Id = 2,
                        Name = "New York",
                        Description = "The Big Apple",
                        PointsOfInterest =
                        {
                            new PointOfInterestDto
                            {
                                Id = 3,
                                Name="Statue of Liberty",
                                Description="Giant green statue"
                            },
                            new PointOfInterestDto
                            {
                                Id = 4,
                                Name="Empire State Building",
                                Description="Skyscraper"
                            }
                        }

                    }
                };
        public List<CityDto> GetCities()
        {
            return Cities;
        }

        public CityDto? GetCity(int cityId)
        {
            return Cities.Find(x => x.Id == cityId);
        }

        public PointOfInterestDto? GetPointOfInterest(int cityId, int id)
        {
            return Cities.Find(city => city.Id == cityId)?
                .PointsOfInterest.FirstOrDefault<PointOfInterestDto>(poi => poi.Id == id);
        }
        
        public CityDto AddCity(CityForCreationDto cityForCreationDto)
        {
            var cityId = CitiesDataStore.Current.Cities.Max(x => x.Id);
            var city = new CityDto()
            {
                Id = ++cityId,
                Name = cityForCreationDto.Name,
                Description = cityForCreationDto.Description
            };
            Cities.Add(city);
            return city;
        }
    }
}
