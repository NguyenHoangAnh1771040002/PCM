using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCM.API.Models
{
    [Table("002_Participants")]
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        // FK to Challenge
        public int ChallengeId { get; set; }
        public virtual Challenge Challenge { get; set; } = null!;

        // FK to Member
        public int MemberId { get; set; }
        public virtual Member Member { get; set; } = null!;

        // Team cho TeamBattle: TeamA, TeamB, None cho RoundRobin/Duel
        public ParticipantTeam Team { get; set; } = ParticipantTeam.None;

        // Đã đóng Entry Fee chưa
        public bool EntryFeePaid { get; set; } = false;

        // Số tiền đã đóng
        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFeeAmount { get; set; } = 0;

        public DateTime JoinedDate { get; set; } = DateTime.Now;

        // Trạng thái: Pending, Confirmed, Withdrawn
        public ParticipantStatus Status { get; set; } = ParticipantStatus.Pending;
    }
}
