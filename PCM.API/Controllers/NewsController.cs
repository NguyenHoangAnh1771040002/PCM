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
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/news
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetAll([FromQuery] bool? isPinned)
        {
            var query = _context.News.AsQueryable();

            if (isPinned.HasValue)
            {
                query = query.Where(n => n.IsPinned == isPinned.Value);
            }

            var news = await query
                .OrderByDescending(n => n.IsPinned)
                .ThenByDescending(n => n.CreatedDate)
                .Select(n => new NewsDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    IsPinned = n.IsPinned,
                    CreatedDate = n.CreatedDate
                })
                .ToListAsync();

            return Ok(news);
        }

        // GET: api/news/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<NewsDto>> GetById(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound(new { message = "Không tìm thấy tin tức" });
            }

            return Ok(new NewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                IsPinned = news.IsPinned,
                CreatedDate = news.CreatedDate
            });
        }

        // POST: api/news
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<NewsDto>> Create([FromBody] NewsCreateDto dto)
        {
            var news = new News
            {
                Title = dto.Title,
                Content = dto.Content,
                IsPinned = dto.IsPinned,
                CreatedDate = DateTime.Now
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = news.Id }, new NewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                IsPinned = news.IsPinned,
                CreatedDate = news.CreatedDate
            });
        }

        // PUT: api/news/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] NewsCreateDto dto)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound(new { message = "Không tìm thấy tin tức" });
            }

            news.Title = dto.Title;
            news.Content = dto.Content;
            news.IsPinned = dto.IsPinned;
            news.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/news/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound(new { message = "Không tìm thấy tin tức" });
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
