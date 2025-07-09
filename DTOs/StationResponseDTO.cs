namespace FuelFinderApi.DTOs
{
    public class StationResponseDTO
    {
        public Guid StationId { get; set; }
        public string? PlaceId { get; set; }
        public string StationName { get; set; }
        public decimal StationLatitude { get; set; }
        public decimal StationLongitude { get; set; }
        public string? StationAddress { get; set; }
        public bool IsApproved { get; set; }
    }

}
