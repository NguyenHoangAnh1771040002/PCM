using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.API.Data;
using PCM.API.DTOs;
using PCM.API.Helpers;
using PCM.API.Models;

namespace PCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/matches
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll(
            [FromQuery] int? challengeId,
            [FromQuery] bool? isRanked,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var query = _context.Matches
                .Include(m => m.Challenge)
                .Include(m => m.Team1_Player1)
                .Include(m => m.Team1_Player2)
                .Include(m => m.Team2_Player1)
                .Include(m => m.Team2_Player2)
                .Include(m => m.Scores)
                .AsQueryable();

            if (challengeId.HasValue)
            {
                query = query.Where(m => m.ChallengeId == challengeId.Value);
            }

            if (isRanked.HasValue)
            {
                query = query.Where(m => m.IsRanked == isRanked.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(m => m.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(m => m.Date <= toDate.Value);
            }

            var matches = await query
                .OrderByDescending(m => m.Date)
                .Select(m => new MatchDto
                {
                    Id = m.Id,
                    Date = m.Date,
                    IsRanked = m.IsRanked,
                    ChallengeId = m.ChallengeId,
                    ChallengeTitle = m.Challenge != null ? m.Challenge.Title : null,
                    MatchFormat = m.MatchFormat,
                    Team1_Player1Id = m.Team1_Player1Id,
                    Team1_Player1Name = m.Team1_Player1.FullName,
                    Team1_Player2Id = m.Team1_Player2Id,
                    Team1_Player2Name = m.Team1_Player2 != null ? m.Team1_Player2.FullName : null,
                    Team2_Player1Id = m.Team2_Player1Id,
                    Team2_Player1Name = m.Team2_Player1.FullName,
                    Team2_Player2Id = m.Team2_Player2Id,
                    Team2_Player2Name = m.Team2_Player2 != null ? m.Team2_Player2.FullName : null,
                    WinningSide = m.WinningSide,
                    CreatedDate = m.CreatedDate,
                    Scores = m.Scores.Select(s => new MatchScoreDto
                    {
                        Id = s.Id,
                        MatchId = s.MatchId,
                        SetNumber = s.SetNumber,
                        Team1Score = s.Team1Score,
                        Team2Score = s.Team2Score,
                        IsFinalSet = s.IsFinalSet
                    }).ToList()
                })
                .ToListAsync();

            return Ok(matches);
        }

        // GET: api/matches/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MatchDto>> GetById(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Challenge)
                .Include(m => m.Team1_Player1)
                .Include(m => m.Team1_Player2)
                .Include(m => m.Team2_Player1)
                .Include(m => m.Team2_Player2)
                .Include(m => m.Scores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound(new { message = "Không tìm thấy trận đấu" });
            }

            return Ok(new MatchDto
            {
                Id = match.Id,
                Date = match.Date,
                IsRanked = match.IsRanked,
                ChallengeId = match.ChallengeId,
                ChallengeTitle = match.Challenge?.Title,
                MatchFormat = match.MatchFormat,
                Team1_Player1Id = match.Team1_Player1Id,
                Team1_Player1Name = match.Team1_Player1.FullName,
                Team1_Player2Id = match.Team1_Player2Id,
                Team1_Player2Name = match.Team1_Player2?.FullName,
                Team2_Player1Id = match.Team2_Player1Id,
                Team2_Player1Name = match.Team2_Player1.FullName,
                Team2_Player2Id = match.Team2_Player2Id,
                Team2_Player2Name = match.Team2_Player2?.FullName,
                WinningSide = match.WinningSide,
                CreatedDate = match.CreatedDate,
                Scores = match.Scores.Select(s => new MatchScoreDto
                {
                    Id = s.Id,
                    MatchId = s.MatchId,
                    SetNumber = s.SetNumber,
                    Team1Score = s.Team1Score,
                    Team2Score = s.Team2Score,
                    IsFinalSet = s.IsFinalSet
                }).ToList()
            });
        }

        // POST: api/matches
        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrReferee)]
        public async Task<ActionResult<MatchDto>> Create([FromBody] MatchCreateDto dto)
        {
            // Validate players
            var players = new List<int> { dto.Team1_Player1Id, dto.Team2_Player1Id };
            if (dto.Team1_Player2Id.HasValue) players.Add(dto.Team1_Player2Id.Value);
            if (dto.Team2_Player2Id.HasValue) players.Add(dto.Team2_Player2Id.Value);

            // Check for duplicate players
            if (players.Count != players.Distinct().Count())
            {
                return BadRequest(new { message = "Không được chọn cùng một người chơi hai lần" });
            }

            // Validate match format
            if (dto.MatchFormat == MatchFormat.Doubles)
            {
                if (!dto.Team1_Player2Id.HasValue || !dto.Team2_Player2Id.HasValue)
                {
                    return BadRequest(new { message = "Trận đôi phải có đủ 4 người chơi" });
                }
            }

            // Validate winning side
            if (dto.WinningSide == WinningSide.None)
            {
                return BadRequest(new { message = "Phải chọn đội thắng" });
            }

            var match = new Match
            {
                Date = dto.Date,
                IsRanked = dto.IsRanked,
                ChallengeId = dto.ChallengeId,
                MatchFormat = dto.MatchFormat,
                Team1_Player1Id = dto.Team1_Player1Id,
                Team1_Player2Id = dto.Team1_Player2Id,
                Team2_Player1Id = dto.Team2_Player1Id,
                Team2_Player2Id = dto.Team2_Player2Id,
                WinningSide = dto.WinningSide,
                CreatedDate = DateTime.Now
            };

            _context.Matches.Add(match);

            // Add scores if provided
            if (dto.Scores != null && dto.Scores.Any())
            {
                foreach (var score in dto.Scores)
                {
                    match.Scores.Add(new MatchScore
                    {
                        SetNumber = score.SetNumber,
                        Team1Score = score.Team1Score,
                        Team2Score = score.Team2Score,
                        IsFinalSet = score.IsFinalSet
                    });
                }
            }

            await _context.SaveChangesAsync();

            // Update ranks if IsRanked
            if (dto.IsRanked)
            {
                await UpdatePlayerRanks(match);
            }

            // Update challenge score if belongs to a challenge
            if (dto.ChallengeId.HasValue)
            {
                await UpdateChallengeScore(dto.ChallengeId.Value, dto.WinningSide, match);
            }

            return CreatedAtAction(nameof(GetById), new { id = match.Id }, await GetById(match.Id));
        }

        // PUT: api/matches/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.AdminOrReferee)]
        public async Task<IActionResult> Update(int id, [FromBody] MatchUpdateDto dto)
        {
            var match = await _context.Matches
                .Include(m => m.Scores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound(new { message = "Không tìm thấy trận đấu" });
            }

            var oldWinningSide = match.WinningSide;
            var oldIsRanked = match.IsRanked;

            match.Date = dto.Date;
            match.IsRanked = dto.IsRanked;
            match.WinningSide = dto.WinningSide;
            match.ModifiedDate = DateTime.Now;

            // Update scores
            if (dto.Scores != null)
            {
                _context.MatchScores.RemoveRange(match.Scores);
                foreach (var score in dto.Scores)
                {
                    match.Scores.Add(new MatchScore
                    {
                        SetNumber = score.SetNumber,
                        Team1Score = score.Team1Score,
                        Team2Score = score.Team2Score,
                        IsFinalSet = score.IsFinalSet
                    });
                }
            }

            await _context.SaveChangesAsync();

            // TODO: Recalculate ranks if needed (complex logic)

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/matches/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Scores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound(new { message = "Không tìm thấy trận đấu" });
            }

            _context.MatchScores.RemoveRange(match.Scores);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }

        private async Task UpdatePlayerRanks(Match match)
        {
            const double RANK_CHANGE = 0.1;

            var winners = new List<int>();
            var losers = new List<int>();

            if (match.WinningSide == WinningSide.Team1)
            {
                winners.Add(match.Team1_Player1Id);
                if (match.Team1_Player2Id.HasValue) winners.Add(match.Team1_Player2Id.Value);

                losers.Add(match.Team2_Player1Id);
                if (match.Team2_Player2Id.HasValue) losers.Add(match.Team2_Player2Id.Value);
            }
            else if (match.WinningSide == WinningSide.Team2)
            {
                winners.Add(match.Team2_Player1Id);
                if (match.Team2_Player2Id.HasValue) winners.Add(match.Team2_Player2Id.Value);

                losers.Add(match.Team1_Player1Id);
                if (match.Team1_Player2Id.HasValue) losers.Add(match.Team1_Player2Id.Value);
            }

            // Update winners
            foreach (var winnerId in winners)
            {
                var member = await _context.Members.FindAsync(winnerId);
                if (member != null)
                {
                    member.RankLevel = Math.Min(7.0, member.RankLevel + RANK_CHANGE);
                    member.TotalMatches++;
                    member.WinMatches++;
                }
            }

            // Update losers
            foreach (var loserId in losers)
            {
                var member = await _context.Members.FindAsync(loserId);
                if (member != null)
                {
                    member.RankLevel = Math.Max(1.0, member.RankLevel - RANK_CHANGE);
                    member.TotalMatches++;
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateChallengeScore(int challengeId, WinningSide winningSide, Match match)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.Id == challengeId);

            if (challenge == null || challenge.GameMode != GameMode.TeamBattle)
            {
                return;
            }

            // Determine which team won (based on player's team in participants)
            var winnerPlayerId = winningSide == WinningSide.Team1 
                ? match.Team1_Player1Id 
                : match.Team2_Player1Id;

            var winnerParticipant = challenge.Participants
                .FirstOrDefault(p => p.MemberId == winnerPlayerId);

            if (winnerParticipant != null)
            {
                if (winnerParticipant.Team == ParticipantTeam.TeamA)
                {
                    challenge.CurrentScore_TeamA++;
                }
                else if (winnerParticipant.Team == ParticipantTeam.TeamB)
                {
                    challenge.CurrentScore_TeamB++;
                }
            }

            // Check if challenge is finished
            if (challenge.Config_TargetWins.HasValue)
            {
                if (challenge.CurrentScore_TeamA >= challenge.Config_TargetWins.Value ||
                    challenge.CurrentScore_TeamB >= challenge.Config_TargetWins.Value)
                {
                    challenge.Status = ChallengeStatus.Finished;
                    challenge.EndDate = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
