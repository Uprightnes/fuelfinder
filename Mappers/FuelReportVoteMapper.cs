using FuelFinderApi.DTOs;
using FuelFinderApi.Models;

namespace FuelFinderApi.Mappers
{
    public static class FuelReportVoteMapper
    {
        public static FuelReportVote ToModel(this FuelReportVoteDTO dto)
        {
            return new FuelReportVote
            {
                FuelReportVoteId = dto.FuelReportVoteId,
                FuelReportId = dto.FuelReportId,
                FuelFinderUserId = dto.FuelFinderUserId,
                IsUpvote = dto.IsUpvote,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = null,
                IsSoftDeleted = dto.IsSoftDeleted
            };
        }

        public static FuelReportVote CreateFromRequest(FuelReportVoteRequestDTO request, Guid reportId, Guid userId)
        {
            return new FuelReportVote
            {
                FuelReportVoteId = Guid.NewGuid(),
                FuelReportId = reportId,
                FuelFinderUserId = userId,
                IsUpvote = request.IsCorrect,
                CreatedOn = DateTime.UtcNow,
                IsSoftDeleted = false
            };
        }

        public static FuelReportVoteDTO ToDTO(FuelReportVote vote)
        {
            return new FuelReportVoteDTO
            {
                FuelReportVoteId = vote.FuelReportVoteId,
                FuelReportId = vote.FuelReportId,
                FuelFinderUserId = vote.FuelFinderUserId,
                IsUpvote = vote.IsUpvote,
                IsSoftDeleted = vote.IsSoftDeleted
            };
        }
    }
}
