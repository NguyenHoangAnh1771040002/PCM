using System.ComponentModel.DataAnnotations;
using PCM.API.Models;

namespace PCM.API.DTOs
{
    // ========== BOOKING DTOs ==========
    public class BookingDto
    {
        public int Id { get; set; }
        public int CourtId { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public BookingStatus Status { get; set; }
        public string StatusName => Status switch
        {
            BookingStatus.Pending => "Chờ xác nhận",
            BookingStatus.Confirmed => "Đã xác nhận",
            BookingStatus.Cancelled => "Đã hủy",
            _ => "Không xác định"
        };
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class BookingCreateDto
    {
        [Required]
        public int CourtId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class BookingUpdateDto
    {
        [Required]
        public int CourtId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public BookingStatus Status { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }

    public class BookingStatusUpdateDto
    {
        [Required]
        public BookingStatus Status { get; set; }
    }

    public class AvailableSlotDto
    {
        public DateTime Date { get; set; }
        public List<TimeSlot> Slots { get; set; } = new();
    }

    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public int? BookingId { get; set; }
        public string? BookedByName { get; set; }
    }

    // ========== CHALLENGE DTOs ==========
    public class ChallengeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ChallengeType Type { get; set; }
        public string TypeName => Type == ChallengeType.Duel ? "Kèo thách đấu" : "Mini-game";
        public GameMode GameMode { get; set; }
        public string GameModeName => GameMode switch
        {
            GameMode.None => "Không",
            GameMode.TeamBattle => "Team Battle",
            GameMode.RoundRobin => "Vòng tròn",
            _ => "Không xác định"
        };
        public ChallengeStatus Status { get; set; }
        public string StatusName => Status switch
        {
            ChallengeStatus.Open => "Đang mở",
            ChallengeStatus.Ongoing => "Đang diễn ra",
            ChallengeStatus.Finished => "Đã kết thúc",
            _ => "Không xác định"
        };
        public int? Config_TargetWins { get; set; }
        public int CurrentScore_TeamA { get; set; }
        public int CurrentScore_TeamB { get; set; }
        public decimal EntryFee { get; set; }
        public decimal PrizePool { get; set; }
        public string? Description { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ParticipantCount { get; set; }
        public List<ParticipantDto> Participants { get; set; } = new();
        public List<MatchDto>? Matches { get; set; }
    }

    public class ChallengeCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ChallengeType Type { get; set; }

        public GameMode GameMode { get; set; } = GameMode.None;

        public int? Config_TargetWins { get; set; }

        public decimal EntryFee { get; set; } = 0;

        public decimal PrizePool { get; set; } = 0;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ChallengeUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public ChallengeStatus Status { get; set; }

        public int? Config_TargetWins { get; set; }

        public decimal EntryFee { get; set; }

        public decimal PrizePool { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    // ========== PARTICIPANT DTOs ==========
    public class ParticipantDto
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public double MemberRank { get; set; }
        public ParticipantTeam Team { get; set; }
        public string TeamName => Team switch
        {
            ParticipantTeam.None => "Chưa chia team",
            ParticipantTeam.TeamA => "Team A",
            ParticipantTeam.TeamB => "Team B",
            _ => "Không xác định"
        };
        public bool EntryFeePaid { get; set; }
        public decimal EntryFeeAmount { get; set; }
        public DateTime JoinedDate { get; set; }
        public ParticipantStatus Status { get; set; }
        public string StatusName => Status switch
        {
            ParticipantStatus.Pending => "Chờ xác nhận",
            ParticipantStatus.Confirmed => "Đã xác nhận",
            ParticipantStatus.Withdrawn => "Đã rút",
            _ => "Không xác định"
        };
    }

    public class JoinChallengeDto
    {
        public bool PayEntryFee { get; set; } = true;
    }
}
