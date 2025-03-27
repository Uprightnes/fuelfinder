using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.userId);

            // Properties Configuration
            builder.Property(u => u.userId)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Auto-generate GUID on add

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Points)
                .HasDefaultValue(0);

            builder.Property(u => u.ReputationScore)
                .HasDefaultValue(100);

            builder.Property(u => u.Otp)
                .HasMaxLength(6); // Assuming 6-digit OTP

            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETDATE()"); // SQL Server syntax

            // Soft delete and status flags
            builder.Property(u => u.isSoftDeleted)
                .HasDefaultValue(false);

            builder.Property(u => u.isActive)
                .HasDefaultValue(true);

            builder.Property(u => u.isVerified)
                .HasDefaultValue(false);

            // Indexes for frequently queried fields
            builder.HasIndex(u => u.Email)
                .IsUnique(); // Ensure email uniqueness

            builder.HasIndex(u => u.Username)
                .IsUnique(); // Ensure username uniqueness

            // Relationships
            builder.HasMany(u => u.FuelReports)
                .WithOne(f => f.User) // Assuming FuelReport has a User navigation property
                .HasForeignKey(f => f.UserId) // Adjust if foreign key name differs
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
