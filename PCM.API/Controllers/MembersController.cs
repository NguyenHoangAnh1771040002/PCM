using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCM.API.Data;
using PCM.API.DTOs;
using PCM.API.Helpers;
using System.Security.Claims;

namespace PCM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/members
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll([FromQuery] string? search, [FromQuery] double? rankMin)
        {
            var query = _context.Members.Where(m => m.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.FullName.Contains(search) || 
                                         (m.Email != null && m.Email.Contains(search)));
            }

            if (rankMin.HasValue)
            {
                query = query.Where(m => m.RankLevel >= rankMin.Value);
            }

            var members = await query
                .OrderByDescending(m => m.RankLevel)
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    DateOfBirth = m.DateOfBirth,
                    JoinDate = m.JoinDate,
                    RankLevel = m.RankLevel,
                    IsActive = m.IsActive,
                    TotalMatches = m.TotalMatches,
                    WinMatches = m.WinMatches
                })
                .ToListAsync();

            return Ok(members);
        }

        // GET: api/members/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MemberDto>> GetById(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound(new { message = "Không tìm thấy hội viên" });
            }

            return Ok(new MemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                DateOfBirth = member.DateOfBirth,
                JoinDate = member.JoinDate,
                RankLevel = member.RankLevel,
                IsActive = member.IsActive,
                TotalMatches = member.TotalMatches,
                WinMatches = member.WinMatches
            });
        }

        // PUT: api/members/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] MemberUpdateDto dto)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound(new { message = "Không tìm thấy hội viên" });
            }

            // Chỉ Admin hoặc chính member mới được sửa
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(RoleConstants.Admin);
            
            if (!isAdmin && member.UserId != userId)
            {
                return Forbid();
            }

            member.FullName = dto.FullName;
            member.Email = dto.Email;
            member.PhoneNumber = dto.PhoneNumber;
            member.DateOfBirth = dto.DateOfBirth;
            member.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // GET: api/members/top-ranking
        [HttpGet("top-ranking")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetTopRanking([FromQuery] int limit = 5)
        {
            var members = await _context.Members
                .Where(m => m.IsActive)
                .OrderByDescending(m => m.RankLevel)
                .Take(limit)
                .Select(m => new MemberDto
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    RankLevel = m.RankLevel,
                    TotalMatches = m.TotalMatches,
                    WinMatches = m.WinMatches
                })
                .ToListAsync();

            return Ok(members);
        }

        // DELETE: api/members/{id} (Admin only - Soft delete)
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound(new { message = "Không tìm thấy hội viên" });
            }

            member.IsActive = false;
            member.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã vô hiệu hóa hội viên" });
        }
    }
}
