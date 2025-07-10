using FuelFinderApi.DTOs;
using FuelFinderApi.Models;

namespace FuelFinderApi.Mappers
{
    public static class Mapper
    {
        public static StationFlag ToModel(this StationFlagRequestDTO dto, Guid stationId, Guid userId)
        {
            return new StationFlag
            {
                StationFlagId = Guid.NewGuid(),
                StationId = stationId,
                FuelFinderUserId = userId,
                Reason = dto.Reason,
                CreatedOn = DateTime.UtcNow,
                IsResolved = false
            };
        }
    }
}