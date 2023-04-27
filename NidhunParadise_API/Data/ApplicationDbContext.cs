using Microsoft.EntityFrameworkCore;
using NidhunParadise_API.Model;
using NidhunParadise_API.Model.Dto;

namespace NidhunParadise_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Beach View Villa",
                    Detials = "Dummy Detail",
                    ImageUrl = "",
                    Occupancy = 4,
                    Rate = 400,
                    Sqft = 300,
                    Amenity = "",
                    CreatedDate= DateTime.Now,
                },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Detials = "Dummy Details",
                    ImageUrl = "",
                    Occupancy = 6,
                    Rate=600,
                    Sqft= 600,
                    Amenity="",
                    CreatedDate= DateTime.Now,
                },
                new Villa
                {
                    Id = 3,
                    Name="Diamond Villa",
                    Detials="Dummy Details",
                    ImageUrl="",
                    Occupancy=7,
                    Rate=800,
                    Sqft= 600,
                    Amenity="",
                    CreatedDate= DateTime.Now,
                });

        }
    }
}
