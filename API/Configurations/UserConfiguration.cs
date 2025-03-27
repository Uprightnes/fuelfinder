using API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace API.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.userId);
            builder.Property(u => u.userId).IsRequired().ValueGeneratedOnAdd();
            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Points).HasDefaultValue(0);
            builder.Property(u => u.ReputationScore).HasDefaultValue(100);
            builder.Property(u => u.Otp).HasMaxLength(6);
            builder.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.isSoftDeleted).HasDefaultValue(false);
            builder.Property(u => u.isActive).HasDefaultValue(true);
            builder.Property(u => u.isVerified).HasDefaultValue(false);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasMany(u => u.FuelReports).WithOne(f => f.User).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
