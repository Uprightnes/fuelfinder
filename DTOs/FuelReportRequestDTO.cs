namespace FuelFinderApi.DTOs
{

    public class FuelReportRequestDTO
    {
        public Guid StationId { get; set; }
        public bool FuelAvailable { get; set; }
        public decimal PricePerLitre { get; set; }
        public int? QueueTime { get; set; }
        public decimal ReportLatitude { get; set; }
        public decimal ReportLongitude { get; set; }
    }

}
