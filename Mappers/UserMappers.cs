using FuelFinderApi.DTOs;
using FuelFinderApi.Models;

namespace FuelFinderApi.Mappers
{
    public static class UserMappers
    {
        public static FuelFinderUser ToModel(this UserRegisterRequestDTO dto, string passwordHash)
        {
            return new FuelFinderUser
            {
                FuelFinderUserId = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Points = 0,
                ReputationScore = 100,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsVerified = false,
                IsSoftDeleted = false
            };
        }

        public static UserResponseDTO ToDto(this FuelFinderUser user)
        {
            return new UserResponseDTO
            {
                FuelFinderUserId = user.FuelFinderUserId,
                Username = user.Username,
                Email = user.Email,
                Points = user.Points,
                ReputationScore = user.ReputationScore,
                IsActive = user.IsActive,
                IsVerified = user.IsVerified
            };
        }

        public static void UpdateRefreshToken(this FuelFinderUser user, string refreshToken, DateTime expiry)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = expiry;
        }
    }
}