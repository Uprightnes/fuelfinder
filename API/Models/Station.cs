using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Station
    {
        [Key]
        public Guid StationId { get; set; }
        
        [Required]
        public string PlaceId { get; set; }
        public string StationName { get; set; }
        [Required]
        [Column(TypeName = "decimal(9,6)")]
        public decimal StationLatitude { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,6)")]
        public decimal StationLongitude { get; set; }
        public string StationAddress { get; set; }
        public ICollection<FuelReport> FuelReports { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool isSoftDeleted { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
