using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.API.Data;
using PCM.API.DTOs;
using PCM.API.Helpers;
using PCM.API.Models;
using System.Security.Claims;

namespace PCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChallengesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/challenges
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ChallengeDto>>> GetAll(
            [FromQuery] ChallengeStatus? status,
            [FromQuery] ChallengeType? type)
        {
            var query = _context.Challenges
                .Include(c => c.CreatedBy)
                .Include(c => c.Participants)
                    .ThenInclude(p => p.Member)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            var challenges = await query
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new ChallengeDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Type = c.Type,
                    GameMode = c.GameMode,
                    Status = c.Status,
                    Config_TargetWins = c.Config_TargetWins,
                    CurrentScore_TeamA = c.CurrentScore_TeamA,
                    CurrentScore_TeamB = c.CurrentScore_TeamB,
                    EntryFee = c.EntryFee,
                    PrizePool = c.PrizePool,
                    Description = c.Description,
                    CreatedById = c.CreatedById,
                    CreatedByName = c.CreatedBy.FullName,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    CreatedDate = c.CreatedDate,
                    ParticipantCount = c.Participants.Count(p => p.Status == ParticipantStatus.Confirmed),
                    Participants = c.Participants.Select(p => new ParticipantDto
                    {
                        Id = p.Id,
                        ChallengeId = p.ChallengeId,
                        MemberId = p.MemberId,
                        MemberName = p.Member.FullName,
                        MemberRank = p.Member.RankLevel,
                        Team = p.Team,
                        EntryFeePaid = p.EntryFeePaid,
                        EntryFeeAmount = p.EntryFeeAmount,
                        JoinedDate = p.JoinedDate,
                        Status = p.Status
                    }).ToList()
                })
                .ToListAsync();

            return Ok(challenges);
        }

        // GET: api/challenges/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ChallengeDto>> GetById(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.CreatedBy)
                .Include(c => c.Participants)
                    .ThenInclude(p => p.Member)
                .Include(c => c.Matches)
                    .ThenInclude(m => m.Team1_Player1)
                .Include(c => c.Matches)
                    .ThenInclude(m => m.Team1_Player2)
                .Include(c => c.Matches)
                    .ThenInclude(m => m.Team2_Player1)
                .Include(c => c.Matches)
                    .ThenInclude(m => m.Team2_Player2)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (challenge == null)
            {
                return NotFound(new { message = "Không tìm thấy challenge" });
            }

            return Ok(new ChallengeDto
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Type = challenge.Type,
                GameMode = challenge.GameMode,
                Status = challenge.Status,
                Config_TargetWins = challenge.Config_TargetWins,
                CurrentScore_TeamA = challenge.CurrentScore_TeamA,
                CurrentScore_TeamB = challenge.CurrentScore_TeamB,
                EntryFee = challenge.EntryFee,
                PrizePool = challenge.PrizePool,
                Description = challenge.Description,
                CreatedById = challenge.CreatedById,
                CreatedByName = challenge.CreatedBy.FullName,
                StartDate = challenge.StartDate,
                EndDate = challenge.EndDate,
                CreatedDate = challenge.CreatedDate,
                ParticipantCount = challenge.Participants.Count(p => p.Status == ParticipantStatus.Confirmed),
                Participants = challenge.Participants.Select(p => new ParticipantDto
                {
                    Id = p.Id,
                    ChallengeId = p.ChallengeId,
                    MemberId = p.MemberId,
                    MemberName = p.Member.FullName,
                    MemberRank = p.Member.RankLevel,
                    Team = p.Team,
                    EntryFeePaid = p.EntryFeePaid,
                    EntryFeeAmount = p.EntryFeeAmount,
                    JoinedDate = p.JoinedDate,
                    Status = p.Status
                }).ToList(),
                Matches = challenge.Matches.Select(m => new MatchDto
                {
                    Id = m.Id,
                    Date = m.Date,
                    IsRanked = m.IsRanked,
                    ChallengeId = m.ChallengeId,
                    MatchFormat = m.MatchFormat,
                    Team1_Player1Id = m.Team1_Player1Id,
                    Team1_Player1Name = m.Team1_Player1.FullName,
                    Team1_Player2Id = m.Team1_Player2Id,
                    Team1_Player2Name = m.Team1_Player2?.FullName,
                    Team2_Player1Id = m.Team2_Player1Id,
                    Team2_Player1Name = m.Team2_Player1.FullName,
                    Team2_Player2Id = m.Team2_Player2Id,
                    Team2_Player2Name = m.Team2_Player2?.FullName,
                    WinningSide = m.WinningSide,
                    CreatedDate = m.CreatedDate
                }).ToList()
            });
        }

        // POST: api/challenges
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ChallengeDto>> Create([FromBody] ChallengeCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return BadRequest(new { message = "Không tìm thấy thông tin hội viên" });
            }

            var challenge = new Challenge
            {
                Title = dto.Title,
                Type = dto.Type,
                GameMode = dto.GameMode,
                Status = ChallengeStatus.Open,
                Config_TargetWins = dto.Config_TargetWins,
                EntryFee = dto.EntryFee,
                PrizePool = dto.PrizePool,
                Description = dto.Description,
                CreatedById = member.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedDate = DateTime.Now
            };

            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = challenge.Id }, new ChallengeDto
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Type = challenge.Type,
                GameMode = challenge.GameMode,
                Status = challenge.Status,
                Config_TargetWins = challenge.Config_TargetWins,
                EntryFee = challenge.EntryFee,
                PrizePool = challenge.PrizePool,
                Description = challenge.Description,
                CreatedById = challenge.CreatedById,
                CreatedByName = member.FullName,
                StartDate = challenge.StartDate,
                EndDate = challenge.EndDate,
                CreatedDate = challenge.CreatedDate
            });
        }

        // PUT: api/challenges/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.AdminOrReferee)]
        public async Task<IActionResult> Update(int id, [FromBody] ChallengeUpdateDto dto)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound(new { message = "Không tìm thấy challenge" });
            }

            challenge.Title = dto.Title;
            challenge.Status = dto.Status;
            challenge.Config_TargetWins = dto.Config_TargetWins;
            challenge.EntryFee = dto.EntryFee;
            challenge.PrizePool = dto.PrizePool;
            challenge.Description = dto.Description;
            challenge.StartDate = dto.StartDate;
            challenge.EndDate = dto.EndDate;
            challenge.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // POST: api/challenges/{id}/join
        [HttpPost("{id}/join")]
        [Authorize]
        public async Task<IActionResult> Join(int id, [FromBody] JoinChallengeDto dto)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound(new { message = "Không tìm thấy challenge" });
            }

            if (challenge.Status != ChallengeStatus.Open)
            {
                return BadRequest(new { message = "Challenge không còn mở đăng ký" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return BadRequest(new { message = "Không tìm thấy thông tin hội viên" });
            }

            // Check if already joined
            var existingParticipant = await _context.Participants
                .FirstOrDefaultAsync(p => p.ChallengeId == id && p.MemberId == member.Id);
            if (existingParticipant != null)
            {
                return BadRequest(new { message = "Bạn đã tham gia challenge này" });
            }

            var participant = new Participant
            {
                ChallengeId = id,
                MemberId = member.Id,
                Team = ParticipantTeam.None,
                EntryFeePaid = dto.PayEntryFee,
                EntryFeeAmount = dto.PayEntryFee ? challenge.EntryFee : 0,
                JoinedDate = DateTime.Now,
                Status = dto.PayEntryFee ? ParticipantStatus.Confirmed : ParticipantStatus.Pending
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã tham gia challenge thành công" });
        }

        // POST: api/challenges/{id}/auto-divide-teams
        [HttpPost("{id}/auto-divide-teams")]
        [Authorize(Roles = RoleConstants.AdminOrReferee)]
        public async Task<IActionResult> AutoDivideTeams(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Participants)
                    .ThenInclude(p => p.Member)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (challenge == null)
            {
                return NotFound(new { message = "Không tìm thấy challenge" });
            }

            if (challenge.GameMode != GameMode.TeamBattle)
            {
                return BadRequest(new { message = "Chỉ áp dụng cho Team Battle" });
            }

            var confirmedParticipants = challenge.Participants
                .Where(p => p.Status == ParticipantStatus.Confirmed)
                .OrderByDescending(p => p.Member.RankLevel)
                .ToList();

            // Chia team theo rank: người rank cao nhất -> Team A, rank thứ 2 -> Team B, ...
            for (int i = 0; i < confirmedParticipants.Count; i++)
            {
                confirmedParticipants[i].Team = i % 2 == 0 ? ParticipantTeam.TeamA : ParticipantTeam.TeamB;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã chia team tự động theo rank" });
        }

        // DELETE: api/challenges/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var challenge = await _context.Challenges
                .Include(c => c.Participants)
                .Include(c => c.Matches)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (challenge == null)
            {
                return NotFound(new { message = "Không tìm thấy challenge" });
            }

            if (challenge.Matches.Any())
            {
                return BadRequest(new { message = "Không thể xóa challenge đã có trận đấu" });
            }

            _context.Participants.RemoveRange(challenge.Participants);
            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
