using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class FuelReport
    {
        [Key]
        public Guid ReportId { get; set; }
        public Guid StationId { get; set; }
        public Station Station { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool FuelAvailable { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerLitre { get; set; }
        public int? QueueTime { get; set; }

        [Column(TypeName = "decimal(9,6)")]
        public decimal Reportlatitude { get; set; } // Changed to decimal

        [Column(TypeName = "decimal(9,6)")]
        public decimal Reportlongitude { get; set; } // Changed to decimal

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool isSoftDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}