using Microsoft.EntityFrameworkCore; 
using API.Models;
using API.Configurations;


namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<User> Users { get; set; }
        //public DbSet<Station> Stations { get; set; }
        //public DbSet<FuelReport> FuelReports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=PHEDITLT34302\\SQLEXPRESS;Database=fuelfinder;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            // Other configurations...
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new FuelReportsConfiguration());
            //modelBuilder.ApplyConfiguration(new StationConfiguration());
            modelBuilder.Entity<Station>().HasQueryFilter(s => !s.isSoftDeleted);
        }
    }
}