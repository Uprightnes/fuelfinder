namespace FuelFinderApi.Models
{
    public class FuelFinderUser 
    {
        public Guid FuelFinderUserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Points { get; set; }
        public int ReputationScore { get; set; } = 100;
        public string? Otp { get; set; } 
        public DateTime CreatedAt { get; set; }
        public bool IsSoftDeleted { get; set; } 
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string? RefreshToken { get; set; } 
        public DateTime? RefreshTokenExpiry { get; set; }
        public ICollection<StationFlag> StationFlags { get; set; } = new List<StationFlag>();
        public ICollection<FuelReport> FuelReports { get; set; } = new List<FuelReport>();
    }
}