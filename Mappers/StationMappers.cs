using FuelFinderApi.DTOs;
using FuelFinderApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FuelFinderApi.Mappers
{
    public static class StationMappers
    {

        public static Station ToModel(this StationResponseDTO dto, string createdBy = "OSM")
        {
            return new Station
            {
                StationId = Guid.NewGuid(),
                StationName = dto.StationName,
                PlaceId = string.IsNullOrEmpty(dto.PlaceId) ? Guid.NewGuid().ToString() : dto.PlaceId,
                StationAddress = dto.StationAddress,
                StationLatitude = dto.StationLatitude,
                StationLongitude = dto.StationLongitude,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                IsSoftDeleted = false,
                IsApproved = createdBy == "OSM"
            };
        }

        public static StationResponseDTO ToDto(this Station station)
        {
            return new StationResponseDTO
            {
                StationId = station.StationId,
                StationName = station.StationName,
                PlaceId = station.PlaceId,
                StationAddress = station.StationAddress,
                StationLatitude = station.StationLatitude,
                StationLongitude = station.StationLongitude,
                IsApproved = station.IsApproved
            };
        }








    }
}