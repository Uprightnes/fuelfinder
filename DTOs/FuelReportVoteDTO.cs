namespace FuelFinderApi.DTOs
{
    public class FuelReportVoteDTO
    {
        public Guid FuelReportVoteId { get; set; }
        public Guid FuelReportId { get; set; }
        public Guid FuelFinderUserId { get; set; }
        public bool IsUpvote { get; set; }
        public bool IsSoftDeleted { get; set; }
    }
}
