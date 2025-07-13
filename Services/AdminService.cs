using FuelFinderApi.Data;
using FuelFinderApi.Exceptions;
using FuelFinderApi.Models;
using FuelFinderApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelFinderApi.Services
{

    public class AdminService : IAdminService
    {
        private readonly ApplicationDBContext _context;
        public AdminService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task ApproveStationAsync(Guid stationId)
        {
            var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == stationId && !s.IsSoftDeleted);

            if (station == null)
            {
                throw new NotFoundException("Station does not exist!");
            }

            station.IsApproved = true;
            station.ModifiedOn = DateTime.UtcNow;
            
            await _context.SaveChangesAsync();

        }

        public async Task ResolveFlagAsync(Guid stationFlagId)
        {
            var flag = await _context.StationFlags.FirstOrDefaultAsync(f => f.StationFlagId == stationFlagId);

            if (flag == null) 
            {
                throw new NotFoundException("Flag does not exist!");
            }

            flag.IsResolved = true;
            await _context.SaveChangesAsync();  


        }
    }
}
