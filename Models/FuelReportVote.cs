namespace FuelFinderApi.Models
{
    public class FuelReportVote
    {
        public Guid FuelReportVoteId { get; set; }
        public Guid FuelReportId { get; set; }
        public Guid FuelFinderUserId { get; set; }
        public bool IsUpvote { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsSoftDeleted { get; set; }
    }
}
