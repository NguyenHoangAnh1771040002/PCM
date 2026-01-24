using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        // FK to Court
        public int CourtId { get; set; }
        public virtual Court Court { get; set; } = null!;

        // FK to Member
        public int MemberId { get; set; }
        public virtual Member Member { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [MaxLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
