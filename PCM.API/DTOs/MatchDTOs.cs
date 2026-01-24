using System.ComponentModel.DataAnnotations;
using PCM.API.Models;

namespace PCM.API.DTOs
{
    // ========== MATCH DTOs ==========
    public class MatchDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsRanked { get; set; }
        public int? ChallengeId { get; set; }
        public string? ChallengeTitle { get; set; }
        public MatchFormat MatchFormat { get; set; }
        public string MatchFormatName => MatchFormat == MatchFormat.Singles ? "Đơn (1vs1)" : "Đôi (2vs2)";

        // Team 1
        public int Team1_Player1Id { get; set; }
        public string Team1_Player1Name { get; set; } = string.Empty;
        public int? Team1_Player2Id { get; set; }
        public string? Team1_Player2Name { get; set; }

        // Team 2
        public int Team2_Player1Id { get; set; }
        public string Team2_Player1Name { get; set; } = string.Empty;
        public int? Team2_Player2Id { get; set; }
        public string? Team2_Player2Name { get; set; }

        public WinningSide WinningSide { get; set; }
        public string WinnerDisplay => WinningSide switch
        {
            WinningSide.None => "Chưa có kết quả",
            WinningSide.Team1 => Team1_Player2Id.HasValue 
                ? $"{Team1_Player1Name} & {Team1_Player2Name}" 
                : Team1_Player1Name,
            WinningSide.Team2 => Team2_Player2Id.HasValue 
                ? $"{Team2_Player1Name} & {Team2_Player2Name}" 
                : Team2_Player1Name,
            _ => "Không xác định"
        };

        public DateTime CreatedDate { get; set; }
        public List<MatchScoreDto> Scores { get; set; } = new();
    }

    public class MatchCreateDto
    {
        public DateTime Date { get; set; } = DateTime.Now;

        public bool IsRanked { get; set; } = true;

        public int? ChallengeId { get; set; }

        [Required]
        public MatchFormat MatchFormat { get; set; }

        [Required]
        public int Team1_Player1Id { get; set; }

        public int? Team1_Player2Id { get; set; }

        [Required]
        public int Team2_Player1Id { get; set; }

        public int? Team2_Player2Id { get; set; }

        [Required]
        public WinningSide WinningSide { get; set; }

        public List<MatchScoreCreateDto>? Scores { get; set; }
    }

    public class MatchUpdateDto
    {
        public DateTime Date { get; set; }
        public bool IsRanked { get; set; }
        public WinningSide WinningSide { get; set; }
        public List<MatchScoreCreateDto>? Scores { get; set; }
    }

    // ========== MATCH SCORE DTOs ==========
    public class MatchScoreDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int SetNumber { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public bool IsFinalSet { get; set; }
    }

    public class MatchScoreCreateDto
    {
        public int SetNumber { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public bool IsFinalSet { get; set; } = false;
    }
}
