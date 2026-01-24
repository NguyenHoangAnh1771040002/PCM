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
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/bookings
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAll(
            [FromQuery] int? courtId,
            [FromQuery] DateTime? date,
            [FromQuery] int? memberId)
        {
            var query = _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.Member)
                .AsQueryable();

            if (courtId.HasValue)
            {
                query = query.Where(b => b.CourtId == courtId.Value);
            }

            if (date.HasValue)
            {
                query = query.Where(b => b.StartTime.Date == date.Value.Date);
            }

            if (memberId.HasValue)
            {
                query = query.Where(b => b.MemberId == memberId.Value);
            }

            var bookings = await query
                .OrderByDescending(b => b.StartTime)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    CourtId = b.CourtId,
                    CourtName = b.Court.Name,
                    MemberId = b.MemberId,
                    MemberName = b.Member.FullName,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    Notes = b.Notes,
                    CreatedDate = b.CreatedDate
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // GET: api/bookings/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy booking" });
            }

            return Ok(new BookingDto
            {
                Id = booking.Id,
                CourtId = booking.CourtId,
                CourtName = booking.Court.Name,
                MemberId = booking.MemberId,
                MemberName = booking.Member.FullName,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Notes = booking.Notes,
                CreatedDate = booking.CreatedDate
            });
        }

        // POST: api/bookings
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BookingDto>> Create([FromBody] BookingCreateDto dto)
        {
            // Validate court
            var court = await _context.Courts.FindAsync(dto.CourtId);
            if (court == null || !court.IsActive)
            {
                return BadRequest(new { message = "Sân không tồn tại hoặc không hoạt động" });
            }

            // Validate time
            if (dto.EndTime <= dto.StartTime)
            {
                return BadRequest(new { message = "Thời gian kết thúc phải sau thời gian bắt đầu" });
            }

            if (dto.StartTime < DateTime.Now)
            {
                return BadRequest(new { message = "Không thể đặt sân cho thời gian đã qua" });
            }

            // Check for overlapping bookings
            var hasOverlap = await _context.Bookings
                .AnyAsync(b => b.CourtId == dto.CourtId &&
                               b.Status != BookingStatus.Cancelled &&
                               b.StartTime < dto.EndTime &&
                               b.EndTime > dto.StartTime);

            if (hasOverlap)
            {
                return BadRequest(new { message = "Khung giờ này đã có người đặt" });
            }

            // Get current member
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            if (member == null)
            {
                return BadRequest(new { message = "Không tìm thấy thông tin hội viên" });
            }

            var booking = new Booking
            {
                CourtId = dto.CourtId,
                MemberId = member.Id,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = BookingStatus.Pending, // Chờ Admin duyệt
                Notes = dto.Notes,
                CreatedDate = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, new BookingDto
            {
                Id = booking.Id,
                CourtId = booking.CourtId,
                CourtName = court.Name,
                MemberId = booking.MemberId,
                MemberName = member.FullName,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status,
                Notes = booking.Notes,
                CreatedDate = booking.CreatedDate
            });
        }

        // PUT: api/bookings/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] BookingUpdateDto dto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy booking" });
            }

            // Check permission
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            var isAdminOrReferee = User.IsInRole(RoleConstants.Admin) || 
                                   User.IsInRole(RoleConstants.Referee) ||
                                   User.IsInRole(RoleConstants.Treasurer);

            if (!isAdminOrReferee && (member == null || booking.MemberId != member.Id))
            {
                return Forbid();
            }

            // Validate and check overlap if changing time/court
            if (dto.CourtId != booking.CourtId || 
                dto.StartTime != booking.StartTime || 
                dto.EndTime != booking.EndTime)
            {
                var hasOverlap = await _context.Bookings
                    .AnyAsync(b => b.Id != id &&
                                   b.CourtId == dto.CourtId &&
                                   b.Status != BookingStatus.Cancelled &&
                                   b.StartTime < dto.EndTime &&
                                   b.EndTime > dto.StartTime);

                if (hasOverlap)
                {
                    return BadRequest(new { message = "Khung giờ này đã có người đặt" });
                }
            }

            booking.CourtId = dto.CourtId;
            booking.StartTime = dto.StartTime;
            booking.EndTime = dto.EndTime;
            booking.Status = dto.Status;
            booking.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // PUT: api/bookings/{id}/status - Chỉ cập nhật trạng thái (Admin)
        [HttpPut("{id}/status")]
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.Referee}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] BookingStatusUpdateDto dto)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy booking" });
            }

            booking.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }

        // DELETE: api/bookings/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound(new { message = "Không tìm thấy booking" });
            }

            // Check permission
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);
            var isAdmin = User.IsInRole(RoleConstants.Admin);

            if (!isAdmin && (member == null || booking.MemberId != member.Id))
            {
                return Forbid();
            }

            // Soft delete - mark as cancelled
            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã hủy booking" });
        }

        // GET: api/bookings/available-slots
        [HttpGet("available-slots")]
        [Authorize]
        public async Task<ActionResult<AvailableSlotDto>> GetAvailableSlots(
            [FromQuery] int courtId,
            [FromQuery] DateTime date)
        {
            var court = await _context.Courts.FindAsync(courtId);
            if (court == null)
            {
                return NotFound(new { message = "Không tìm thấy sân" });
            }

            // Get existing bookings for the day
            var existingBookings = await _context.Bookings
                .Where(b => b.CourtId == courtId &&
                            b.StartTime.Date == date.Date &&
                            b.Status != BookingStatus.Cancelled)
                .Include(b => b.Member)
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            // Generate time slots (6:00 - 22:00, each slot 1 hour)
            var slots = new List<TimeSlot>();
            var startHour = 6;
            var endHour = 22;

            for (int hour = startHour; hour < endHour; hour++)
            {
                var slotStart = date.Date.AddHours(hour);
                var slotEnd = date.Date.AddHours(hour + 1);

                var overlappingBooking = existingBookings
                    .FirstOrDefault(b => b.StartTime < slotEnd && b.EndTime > slotStart);

                slots.Add(new TimeSlot
                {
                    StartTime = slotStart,
                    EndTime = slotEnd,
                    IsAvailable = overlappingBooking == null,
                    BookingId = overlappingBooking?.Id,
                    BookedByName = overlappingBooking?.Member.FullName
                });
            }

            return Ok(new AvailableSlotDto
            {
                Date = date.Date,
                Slots = slots
            });
        }

        // GET: api/bookings/my-bookings
        [HttpGet("my-bookings")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);

            if (member == null)
            {
                return Ok(new List<BookingDto>());
            }

            var bookings = await _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.Member)
                .Where(b => b.MemberId == member.Id)
                .OrderByDescending(b => b.StartTime)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    CourtId = b.CourtId,
                    CourtName = b.Court.Name,
                    MemberId = b.MemberId,
                    MemberName = b.Member.FullName,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Status = b.Status,
                    Notes = b.Notes,
                    CreatedDate = b.CreatedDate
                })
                .ToListAsync();

            return Ok(bookings);
        }
    }
}
