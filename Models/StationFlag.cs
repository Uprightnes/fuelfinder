namespace FuelFinderApi.Models
{
    public class StationFlag
    {
        public Guid StationFlagId { get; set; }
        public Guid StationId { get; set; }
        public Station Station { get; set; } = null!;
        public Guid FuelFinderUserId { get; set; }
        public FuelFinderUser FuelFinderUser { get; set; } = null!;
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public bool IsResolved { get; set; }
    }
}
