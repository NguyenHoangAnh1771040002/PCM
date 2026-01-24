using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    // Bảng bổ sung - Lưu chi tiết điểm từng set
    [Table("002_MatchScores")]
    public class MatchScore
    {
        [Key]
        public int Id { get; set; }

        // FK to Match
        public int MatchId { get; set; }
        public virtual Match Match { get; set; } = null!;

        // Số set (1, 2, 3,...)
        public int SetNumber { get; set; }

        public int Team1Score { get; set; }
        public int Team2Score { get; set; }

        public bool IsFinalSet { get; set; } = false;
    }
}
