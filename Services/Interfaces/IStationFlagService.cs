using FuelFinderApi.DTOs;

namespace FuelFinderApi.Services.Interfaces
{
    public interface IStationFlagService
    {
        Task FlagStationAsync(Guid stationId, StationFlagRequestDTO request, Guid userId);
    }
}
