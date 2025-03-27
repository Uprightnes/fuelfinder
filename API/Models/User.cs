using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Users")]
    public class User
    {

        public Guid userId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Points { get; set; }
        public int ReputationScore { get; set; } = 100;
        public string Otp {  get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<FuelReport> FuelReports { get; set; }
        public bool isSoftDeleted { get; set; }
        public bool isActive { get; set; } 
        public bool isVerified {  get; set; }
    }
}
