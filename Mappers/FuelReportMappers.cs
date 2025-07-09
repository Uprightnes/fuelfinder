using FuelFinderApi.DTOs;
using FuelFinderApi.Models;

namespace FuelFinderApi.Mappers
{
    public static class FuelReportMappers
    {
        public static FuelReport ToModel(this FuelReportRequestDTO dto, Guid userId, string username, Station station)
        {
            return new FuelReport
            {
                FuelReportId = Guid.NewGuid(),
                StationId = dto.StationId,
                FuelFinderUserId = userId,
                FuelAvailable = dto.FuelAvailable,
                PricePerLitre = dto.PricePerLitre,
                QueueTime = dto.QueueTime,
                ReportLatitude = dto.ReportLatitude,
                ReportLongitude = dto.ReportLongitude,
                CreatedBy = username,
                CreatedOn = DateTime.UtcNow,
                IsSoftDeleted = false,
                IsCorrect = true,
                VoteCount = 0
            };
        }

        public static FuelReportResponseDTO ToDto(this FuelReport report, string stationName, string username)
        {
            return new FuelReportResponseDTO
            {
                FuelReportId = report.FuelReportId,
                StationId = report.StationId,
                StationName = stationName,
                FuelFinderUserId = report.FuelFinderUserId,
                Username = username,
                FuelAvailable = report.FuelAvailable,
                PricePerLitre = report.PricePerLitre,
                QueueTime = report.QueueTime,
                ReportLatitude = report.ReportLatitude,
                ReportLongitude = report.ReportLongitude,
                CreatedOn = report.CreatedOn,
                IsCorrect = report.IsCorrect,
                VoteCount = report.VoteCount
            };
        }
    }
}