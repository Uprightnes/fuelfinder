namespace FuelFinderApi.DTOs
{
    public class UserResponseDTO
    {
        public Guid FuelFinderUserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int ReputationScore { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
    }
}
