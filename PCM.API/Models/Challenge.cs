using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_Challenges")]
    public class Challenge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // Loại: Duel hoặc MiniGame
        public ChallengeType Type { get; set; }

        // Thể thức: None (cho Duel), TeamBattle, RoundRobin
        public GameMode GameMode { get; set; } = GameMode.None;

        // Trạng thái: Open, Ongoing, Finished
        public ChallengeStatus Status { get; set; } = ChallengeStatus.Open;

        // Cấu hình cho TeamBattle: Số trận thắng cần đạt
        public int? Config_TargetWins { get; set; }

        // Điểm hiện tại của 2 phe (cho TeamBattle)
        public int CurrentScore_TeamA { get; set; } = 0;
        public int CurrentScore_TeamB { get; set; } = 0;

        // Số tiền mỗi người đóng để tham gia
        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFee { get; set; } = 0;

        // Tổng quỹ thưởng
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrizePool { get; set; } = 0;

        // Mô tả
        [MaxLength(1000)]
        public string? Description { get; set; }

        // FK to Member (người tạo)
        public int CreatedById { get; set; }
        public virtual Member CreatedBy { get; set; } = null!;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
