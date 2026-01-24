using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    // Bảng bổ sung - Thông báo
    [Table("002_Notifications")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        // FK to Member (nullable = thông báo chung cho tất cả)
        public int? MemberId { get; set; }
        public virtual Member? Member { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public NotificationType Type { get; set; } = NotificationType.Info;

        public bool IsRead { get; set; } = false;

        [MaxLength(500)]
        public string? LinkUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
