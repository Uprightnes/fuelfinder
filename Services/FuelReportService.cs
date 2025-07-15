using System.Security.Claims;
using FuelFinderApi.Data;
using FuelFinderApi.DTOs;
using FuelFinderApi.Mappers;
using FuelFinderApi.Models;
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

        public async Task<IEnumerable<FuelReportResponseDTO>> GetReportsAsync(int page, int size, Guid? stationId)
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


        public async Task VoteOnReportAsync(Guid reportId, FuelReportVoteRequestDTO request, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub") ?? user.FindFirst("userId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                throw new UnauthorizedAccessException("User ID not found or invalid.");

            var report = await _context.FuelReports
                .Include(r => r.FuelFinderUser)
                .FirstOrDefaultAsync(r => r.FuelReportId == reportId && !r.IsSoftDeleted);
            if (report == null)
                throw new KeyNotFoundException("Report not found.");

            if (report.FuelFinderUserId == userId)
                throw new UnauthorizedAccessException("Cannot vote on your own report.");

            var existingVote = await _context.FuelReportVotes
                .FirstOrDefaultAsync(v => v.FuelReportId == reportId && v.FuelFinderUserId == userId && !v.IsSoftDeleted);
            if (existingVote != null)
            {
                if (existingVote.IsUpvote == request.IsCorrect)
                    return;
                report.VoteCount += request.IsCorrect ? 2 : -2; 
                existingVote.IsUpvote = request.IsCorrect;
                existingVote.ModifiedOn = DateTime.UtcNow;
            }
            else
            {
                var vote = FuelReportVoteMapper.CreateFromRequest(request, reportId, userId);
                _context.FuelReportVotes.Add(vote);
                report.VoteCount += request.IsCorrect ? 1 : -1;
            }

            report.IsCorrect = report.VoteCount >= 5;
            report.FuelFinderUser.ReputationScore += request.IsCorrect ? 5 : -10;
            report.FuelFinderUser.ReputationScore = Math.Max(0, report.FuelFinderUser.ReputationScore);

            await _context.SaveChangesAsync();
        }

      
    }
}
