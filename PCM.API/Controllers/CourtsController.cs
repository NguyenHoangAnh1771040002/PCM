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
    public class CourtsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/courts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CourtDto>>> GetAll()
        {
            var courts = await _context.Courts
                .OrderBy(c => c.Name)
                .Select(c => new CourtDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    Description = c.Description
                })
                .ToListAsync();

            return Ok(courts);
        }

        // GET: api/courts/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourtDto>> GetById(int id)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court == null)
            {
                return NotFound(new { message = "Không tìm thấy sân" });
            }

            return Ok(new CourtDto
            {
                Id = court.Id,
                Name = court.Name,
                IsActive = court.IsActive,
                Description = court.Description
            });
        }

        // POST: api/courts
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<CourtDto>> Create([FromBody] CourtCreateDto dto)
        {
            var court = new Court
            {
                Name = dto.Name,
                IsActive = dto.IsActive,
                Description = dto.Description,
                CreatedDate = DateTime.Now
            };

            _context.Courts.Add(court);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = court.Id }, new CourtDto
            {
                Id = court.Id,
                Name = court.Name,
                IsActive = court.IsActive,
                Description = court.Description
            });
        }

        // PUT: api/courts/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] CourtCreateDto dto)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court == null)
            {
                return NotFound(new { message = "Không tìm thấy sân" });
            }

            court.Name = dto.Name;
            court.IsActive = dto.IsActive;
            court.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/courts/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var court = await _context.Courts.FindAsync(id);
            if (court == null)
            {
                return NotFound(new { message = "Không tìm thấy sân" });
            }

            // Kiểm tra xem có booking nào không
            var hasBookings = await _context.Bookings.AnyAsync(b => b.CourtId == id);
            if (hasBookings)
            {
                // Soft delete - đánh dấu không hoạt động
                court.IsActive = false;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Đã vô hiệu hóa sân (có lịch đặt trước đó)" });
            }

            _context.Courts.Remove(court);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
