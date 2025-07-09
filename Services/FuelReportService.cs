using FuelFinderApi.Data;
using FuelFinderApi.DTOs;
using FuelFinderApi.Mappers;
using FuelFinderApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelFinderApi.Services
{
    public class FuelReportService : IFuelReportService
    {
        private readonly ApplicationDBContext _context;

        public FuelReportService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<FuelReportResponseDTO> SubmitReportAsync(FuelReportRequestDTO request, Guid userId)
        {
            if (request.PricePerLitre < 0 || request.QueueTime < 0)
            {
                throw new ArgumentException("Price and queue time cannot be negative.");
            }

            var station = await _context.Stations.FirstOrDefaultAsync(s => s.StationId == request.StationId && !s.IsSoftDeleted);
            if (station == null)
            {
                throw new KeyNotFoundException("Station not found.");
            }

            var user = await _context.FuelFinderUsers.FirstOrDefaultAsync(u => u.FuelFinderUserId == userId && !u.IsSoftDeleted);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var report = request.ToModel(userId, user.Username, station); 
            _context.FuelReports.Add(report);
            await _context.SaveChangesAsync();
            return report.ToDto(station.StationName, user.Username); 
        }

        public async Task<IEnumerable<FuelReportResponseDTO>> GetReportsAsync(Guid? stationId)
        {
            var query = _context.FuelReports
                .Include(r => r.Station)
                .Include(r => r.FuelFinderUser)
                .Where(r => !r.IsSoftDeleted);

            if (stationId.HasValue)
            {
                query = query.Where(r => r.StationId == stationId.Value);
            }

            return await query
                .Select(r => r.ToDto(r.Station.StationName, r.FuelFinderUser.Username)) 
            .ToListAsync();
        }

        public async Task VoteOnReportAsync(Guid reportId, FuelReportVoteRequestDTO request)
        {
            var report = await _context.FuelReports
                .Include(r => r.FuelFinderUser)
                .FirstOrDefaultAsync(r => r.FuelReportId == reportId && !r.IsSoftDeleted);
            if (report == null)
            {
                throw new KeyNotFoundException("Report not found.");
            }

            report.VoteCount++;
            if (!request.IsCorrect)
            {
                report.IsCorrect = false;
                report.FuelFinderUser.ReputationScore = Math.Max(0, report.FuelFinderUser.ReputationScore - 10);
            }
            else
            {
                report.FuelFinderUser.ReputationScore += 5;
            }

            await _context.SaveChangesAsync();
        }
    }
}
