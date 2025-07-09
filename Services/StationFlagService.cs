using FuelFinderApi.Data;
using FuelFinderApi.DTOs;
using FuelFinderApi.Mappers;
using FuelFinderApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelFinderApi.Services
{
   
    public class StationFlagService : IStationFlagService
    {
        private readonly ApplicationDBContext _context;

        public StationFlagService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task FlagStationAsync(Guid stationId, StationFlagRequestDTO request, Guid userId)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == stationId && !s.IsSoftDeleted);
            if (station == null)
            {
                throw new KeyNotFoundException("Station not found.");
            }

            var user = await _context.FuelFinderUsers.FirstOrDefaultAsync(u => u.FuelFinderUserId == userId && !u.IsSoftDeleted);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var flag = request.ToModel(stationId, userId);
            _context.StationFlags.Add(flag);
            await _context.SaveChangesAsync();
        }
    }

}
