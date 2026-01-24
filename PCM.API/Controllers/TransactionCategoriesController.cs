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
    [Route("api/transaction-categories")]
    public class TransactionCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/transaction-categories
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TransactionCategoryDto>>> GetAll([FromQuery] TransactionType? type)
        {
            var query = _context.TransactionCategories.AsQueryable();

            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            var categories = await query
                .OrderBy(c => c.Type)
                .ThenBy(c => c.Name)
                .Select(c => new TransactionCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/transaction-categories/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TransactionCategoryDto>> GetById(int id)
        {
            var category = await _context.TransactionCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            return Ok(new TransactionCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type
            });
        }

        // POST: api/transaction-categories
        [HttpPost]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult<TransactionCategoryDto>> Create([FromBody] TransactionCategoryCreateDto dto)
        {
            var category = new TransactionCategory
            {
                Name = dto.Name,
                Type = dto.Type,
                CreatedDate = DateTime.Now
            };

            _context.TransactionCategories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, new TransactionCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Type = category.Type
            });
        }

        // PUT: api/transaction-categories/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] TransactionCategoryCreateDto dto)
        {
            var category = await _context.TransactionCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            category.Name = dto.Name;
            category.Type = dto.Type;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công" });
        }

        // DELETE: api/transaction-categories/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.TransactionCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Không tìm thấy danh mục" });
            }

            // Kiểm tra xem có transaction nào sử dụng không
            var hasTransactions = await _context.Transactions.AnyAsync(t => t.CategoryId == id);
            if (hasTransactions)
            {
                return BadRequest(new { message = "Không thể xóa danh mục đang có giao dịch" });
            }

            _context.TransactionCategories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
