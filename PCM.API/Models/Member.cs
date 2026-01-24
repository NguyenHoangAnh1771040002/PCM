using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_Members")]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;

        // Rank DUPR (Trình độ), mặc định 3.0 cho người mới
        public double RankLevel { get; set; } = 3.0;

        public bool IsActive { get; set; } = true;

        // Thống kê trận đấu
        public int TotalMatches { get; set; } = 0;
        public int WinMatches { get; set; } = 0;

        // FK to Identity User
        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Challenge> CreatedChallenges { get; set; } = new List<Challenge>();
        public virtual ICollection<Participant> Participations { get; set; } = new List<Participant>();
    }
}
