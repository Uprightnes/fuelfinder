using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations
{
    public class FuelReportsConfiguration : IEntityTypeConfiguration<FuelReport>
    {
        public void Configure(EntityTypeBuilder<FuelReport> builder)
        {
            // Table name
            builder.ToTable("FuelReports");

            // Primary Key
            builder.HasKey(f => f.ReportId);
            builder.Property(f => f.ReportId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Foreign Keys & Relationships
            builder.HasOne(f => f.Station)
                .WithMany() // Assuming Station has no collection of FuelReports
                .HasForeignKey(f => f.StationId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent station deletion if reports exist

            builder.HasOne(f => f.User)
                .WithMany(u => u.FuelReports)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete reports when user is deleted

            // Properties
            builder.Property(f => f.FuelAvailable)
                .IsRequired();

            builder.Property(f => f.PricePerLitre)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(f => f.QueueTime)
                .IsRequired(false); // Nullable

            builder.Property(f => f.Reportlatitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired();

            builder.Property(f => f.Reportlongitude)
                .HasColumnType("decimal(9,6)")
                .IsRequired(); // SQL Server

            // Audit Fields
            builder.Property(f => f.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(f => f.CreatedOn)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(f => f.isSoftDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(f => f.ModifiedBy)
                .HasMaxLength(255)
                .IsRequired(false); // Nullable

            builder.Property(f => f.ModifiedOn)
                .IsRequired(false); // Nullable

            // Indexes
            builder.HasIndex(f => f.StationId);
            builder.HasIndex(f => f.UserId);
        }
    }
}
