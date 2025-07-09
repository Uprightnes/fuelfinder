namespace FuelFinderApi.Models
{
    public class Station
    {
        public Guid StationId { get; set; }
        public string? PlaceId { get; set; } 
        public string StationName { get; set; }
        public decimal StationLatitude { get; set; }
        public decimal StationLongitude { get; set; }
        public string? StationAddress { get; set; } 
        public string? CreatedBy { get; set; } 
        public DateTime CreatedOn { get; set; }
        public bool IsSoftDeleted { get; set; }
        public string? ModifiedBy { get; set; } 
        public DateTime? ModifiedOn { get; set; }
        public bool IsApproved { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public ICollection<StationFlag> StationFlags { get; set; } = new List<StationFlag>();

        public ICollection<FuelReport> FuelReports { get; set; } = new List<FuelReport>();
    }
}