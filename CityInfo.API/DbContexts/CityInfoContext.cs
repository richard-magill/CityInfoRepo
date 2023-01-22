using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext: DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options): base(options)
        {

        }

        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The city that never sleeps"
                },
                new City("Seattle")
                {
                    Id = 2,
                    Description = "PNW"
                });
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Empire State Building")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Skyscraper"
                },
                new PointOfInterest("Central Park")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Big pretty park"
                },
                new PointOfInterest("Space Needle")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "Futuristic tower"
                },
                new PointOfInterest("Pike Place Market")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "Flying fish"
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
