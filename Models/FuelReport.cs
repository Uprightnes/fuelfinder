namespace FuelFinderApi.Models
{
    public class FuelReport
    {
        public Guid FuelReportId { get; set; }
        public Guid StationId { get; set; }
        public Station Station { get; set; }
        public Guid FuelFinderUserId { get; set; }
        public FuelFinderUser FuelFinderUser { get; set; }
        public bool FuelAvailable { get; set; }
        public decimal PricePerLitre { get; set; }
        public int? QueueTime { get; set; } 
        public decimal ReportLatitude { get; set; } 
        public decimal ReportLongitude { get; set; } 
        public string? CreatedBy { get; set; } 
        public DateTime CreatedOn { get; set; }
        public bool IsSoftDeleted { get; set; }
        public string? ModifiedBy { get; set; } 
        public DateTime? ModifiedOn { get; set; } 
        public bool IsCorrect { get; set; } 
        public int VoteCount { get; set; } 
    }
}