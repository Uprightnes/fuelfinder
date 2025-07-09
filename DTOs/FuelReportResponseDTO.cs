namespace FuelFinderApi.DTOs
{
    public class FuelReportResponseDTO
    {
        public Guid FuelReportId { get; set; }
        public Guid StationId { get; set; }
        public string StationName { get; set; }
        public Guid FuelFinderUserId { get; set; }
        public string Username { get; set; }
        public bool FuelAvailable { get; set; }
        public decimal PricePerLitre { get; set; }
        public int? QueueTime { get; set; }
        public decimal ReportLatitude { get; set; }
        public decimal ReportLongitude { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsCorrect { get; set; }
        public int VoteCount { get; set; }
    }

}
