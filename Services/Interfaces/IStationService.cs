using FuelFinderApi.DTOs;

namespace FuelFinderApi.Services.Interfaces
{
    public interface IStationService
    {
        Task<IEnumerable<StationResponseDTO>> GetNearbyStationsAsync(decimal latitude, decimal longitude);
        Task<StationResponseDTO> AddStationAsync(StationResponseDTO request, Guid userId);
        
    }
}
