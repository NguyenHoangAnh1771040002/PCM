using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_Matches")]
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        // Quan trọng: Nếu = true thì trận này sẽ ảnh hưởng đến Rank DUPR
        public bool IsRanked { get; set; } = true;

        // FK to Challenge (nullable - null nếu là trận giao hữu)
        public int? ChallengeId { get; set; }
        public virtual Challenge? Challenge { get; set; }

        // Thể thức: Singles (1vs1) hoặc Doubles (2vs2)
        public MatchFormat MatchFormat { get; set; }

        // ĐỘI HÌNH 1 (TEAM 1)
        public int Team1_Player1Id { get; set; }
        [ForeignKey("Team1_Player1Id")]
        public virtual Member Team1_Player1 { get; set; } = null!;

        public int? Team1_Player2Id { get; set; }  // Chỉ có dữ liệu khi Doubles
        [ForeignKey("Team1_Player2Id")]
        public virtual Member? Team1_Player2 { get; set; }

        // ĐỘI HÌNH 2 (TEAM 2)
        public int Team2_Player1Id { get; set; }
        [ForeignKey("Team2_Player1Id")]
        public virtual Member Team2_Player1 { get; set; } = null!;

        public int? Team2_Player2Id { get; set; }  // Chỉ có dữ liệu khi Doubles
        [ForeignKey("Team2_Player2Id")]
        public virtual Member? Team2_Player2 { get; set; }

        // KẾT QUẢ
        public WinningSide WinningSide { get; set; } = WinningSide.None;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<MatchScore> Scores { get; set; } = new List<MatchScore>();
    }
}
