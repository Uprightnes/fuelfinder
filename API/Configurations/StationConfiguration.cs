using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations
{
    public class StationConfiguration : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.ToTable("Stations");

            // Primary Key
            builder.HasKey(s => s.StationId);
            builder.Property(s => s.StationId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Properties
            builder.Property(s => s.PlaceId)
                .IsRequired()
                .HasMaxLength(255); // Adjust length based on your PlaceId format

            builder.Property(s => s.StationName)
                .HasMaxLength(100);

            builder.Property(s => s.StationLatitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(s => s.StationLongitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(s => s.StationAddress)
                .HasMaxLength(500);

            // Relationships
            builder.HasMany(s => s.FuelReports)
                .WithOne(f => f.Station)
                .HasForeignKey(f => f.StationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent station deletion if reports exist

            // Audit Fields
            builder.Property(s => s.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(s => s.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()"); // Use CURRENT_TIMESTAMP for other databases

            builder.Property(s => s.isSoftDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(s => s.ModifiedBy)
                .HasMaxLength(255)
                .IsRequired(false); // Nullable

            builder.Property(s => s.ModifiedOn)
                .IsRequired(false); // Nullable

            // Indexes
            builder.HasIndex(s => s.PlaceId)
                .IsUnique(); // Assuming PlaceId should be unique

            builder.HasIndex(s => new { s.StationLatitude, s.StationLongitude });
        }
    }
}
