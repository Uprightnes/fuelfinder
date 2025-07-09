using FuelFinderApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuelFinderApi.Data
{
    public class ApplicationDBContext : IdentityDbContext<FuelFinderUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<FuelFinderUser> FuelFinderUsers { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<FuelReport> FuelReports { get; set; }
        public DbSet<StationFlag> StationFlags { get; set; }
    }
}
