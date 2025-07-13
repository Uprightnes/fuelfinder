using FuelFinderApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FuelFinderApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<FuelFinderUser> FuelFinderUsers { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<FuelReport> FuelReports { get; set; }
        public DbSet<StationFlag> StationFlags { get; set; }
        public DbSet<FuelReportVote> FuelReportVotes { get; set; }
    }
}
